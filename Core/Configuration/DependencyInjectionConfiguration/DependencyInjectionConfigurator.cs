﻿using System.Reflection;
using Maestro.Core.Logging;
using Maestro.Core.Providers;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace Maestro.Core.Configuration.DependencyInjectionConfiguration;

public static class DependencyInjectionConfigurator
{
    private const string settingsName = "appsettings.json";
    private static readonly Lifestyle LifestyleByDefault = Lifestyle.Singleton;

    public static Container Configure(this Container container)
    {
        var assemblies = AssemblyHelper.GetServiceAssemblies();

        var registrations = assemblies
            .SelectMany(x => x.GetExportedTypes())
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract)
            .SelectMany(
                x => x.GetInterfaces()
                    .Where(i => assemblies.Contains(i.Assembly))
                    .Select(i => new Registration(i, x, LifestyleByDefault))
            )
            .Where(x => x.Resolve())
            .Select(x => x.GetGenericDefinitionTypeIfNeed())
            .ToArray();

        foreach (var registration in registrations)
        {
            var scopeAttribute = registration.GetScopeAttribute();
            container.Register(
                registration.InterfaceType,
                registration.ImplementationType,
                scopeAttribute?.Lifestyle ?? registration.Lifestyle
            );
        }

        return container;
    }

    public static Container ConfigureSettings(this Container container)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(settingsName, optional: false, reloadOnChange: true)
            .SetFileLoadExceptionHandler(HandleFileLoadException)
            .Build();
        container.RegisterInstance<ISettingsProvider>(new SettingsProvider(configuration));
        return container;

        void HandleFileLoadException(FileLoadExceptionContext fileLoadExceptionContext)
        {
            if (fileLoadExceptionContext.Exception is not FileNotFoundException)
            {
                throw fileLoadExceptionContext.Exception;
            }

#pragma warning disable CS0618 // Type or member is obsolete
            var log = LogFactory.ClosedFactory.ForContext<ConfiguratorBase>();
#pragma warning restore CS0618 // Type or member is obsolete
            log.Warn(fileLoadExceptionContext.Exception.Message);
            fileLoadExceptionContext.Ignore = true;
        }
    }

    public static Container ConfigureApplication(this Container container)
    {
        var applicationType = Assembly
            .GetEntryAssembly()!
            .GetExportedTypes()
            .Single(x => x.GetInterfaces().Contains(typeof(IApplication)));

        container.RegisterSingleton(applicationType, applicationType);
        return container;
    }
}