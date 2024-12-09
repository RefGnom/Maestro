using System.Reflection;
using Maestro.Core.Extensions;
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

        assemblies.SelectMany(x => x.GetExportedTypes())
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract)
            .SelectMany(
                x => x.GetInterfaces()
                    .Where(i => assemblies.Contains(i.Assembly))
                    .Select(i => new Registration(i, x, LifestyleByDefault))
            )
            .Where(x => x.Resolve())
            .Select(x => x.GetGenericDefinitionTypeIfNeed())
            .GroupBy(x => x.InterfaceType)
            .ForEach(container.RegisterGroup);

        return container;
    }

    public static Container ConfigureSettings(this Container container)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(settingsName, optional: true, reloadOnChange: true)
            .Build();
        container.RegisterInstance<ISettingsProvider>(new SettingsProvider(configuration));
        return container;
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

    private static void RegisterGroup(this Container container, IEnumerable<Registration> registrationGroup)
    {
        var registrations = registrationGroup.ToArray();
        var registrationMaster = registrations.DistinctBy(x => x.InterfaceType).Single();
        var scopeAttribute = registrationMaster.GetScopeAttribute();

        if (registrations.Length == 1)
        {
            container.Register(
                registrationMaster.InterfaceType,
                registrationMaster.ImplementationType,
                scopeAttribute?.Lifestyle ?? registrationMaster.Lifestyle
            );
        }

        container.Collection.Register(
            registrationMaster.InterfaceType,
            registrations.Select(x => x.ImplementationType),
            scopeAttribute?.Lifestyle ?? registrationMaster.Lifestyle
        );
    }
}