using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.Masonry.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Registrars;

namespace Soenneker.Blazor.Masonry.Registrars;

/// <summary>
/// A Blazor interop library that integrates Masonry (https://masonry.desandro.com), the cascading grid layout library
/// </summary>
public static class MasonryRegistrar
{
    /// <summary>
    /// Adds masonry interop as scoped.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The result of the operation.</returns>
    public static IServiceCollection AddMasonryInteropAsScoped(this IServiceCollection services)
    {
        services.AddResourceLoaderAsScoped().TryAddScoped<IMasonryInterop, MasonryInterop>();

        return services;
    }
}
