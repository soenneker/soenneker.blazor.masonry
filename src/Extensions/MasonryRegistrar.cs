using Microsoft.Extensions.DependencyInjection;
using Soenneker.Blazor.Masonry.Abstract;

namespace Soenneker.Blazor.Masonry.Extensions;

public static class MasonryRegistrar
{
    /// <summary>
    /// Shorthand for <code>services.AddScoped</code>
    /// </summary>
    public static void AddMasonry(this IServiceCollection services)
    {
        services.AddScoped<IMasonryInterop, MasonryInterop>();
    }
}