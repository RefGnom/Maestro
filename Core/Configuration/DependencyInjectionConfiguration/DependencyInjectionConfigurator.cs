using System.Reflection;
using Maestro.Core.Extensions;
using Maestro.Core.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Maestro.Core.Configuration.DependencyInjectionConfiguration;

public static class DependencyInjectionConfigurator
{
    private const string SettingsName = "appsettings.json";
    private const ServiceLifetime LifestyleByDefault = ServiceLifetime.Singleton;

    public static IServiceCollection Configure(this IServiceCollection container)
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

    public static IServiceCollection ConfigureSettings(this IServiceCollection container)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(SettingsName, optional: true, reloadOnChange: true)
            .Build();
        container.AddSingleton<ISettingsProvider>(new SettingsProvider(configuration));
        return container;
    }

    public static IServiceCollection ConfigureApplication(this IServiceCollection container)
    {
        var applicationType = Assembly
            .GetEntryAssembly()!
            .GetExportedTypes()
            .Single(x => x.GetInterfaces().Contains(typeof(IApplication)));

        container.AddSingleton(applicationType, applicationType);
        return container;
    }

    private static void RegisterGroup(this IServiceCollection container, IEnumerable<Registration> registrationGroup)
    {
        var registrations = registrationGroup.ToArray();
        var registrationMaster = registrations.DistinctBy(x => x.InterfaceType).Single();
        var scopeAttribute = registrationMaster.GetScopeAttribute();

        foreach (var registration in registrations)
        {
            var descriptor = new ServiceDescriptor(
                registration.InterfaceType,
                registration.ImplementationType,
                scopeAttribute?.Lifestyle ?? registrationMaster.Lifestyle
            );
            container.Add(descriptor);
        }
    }
}