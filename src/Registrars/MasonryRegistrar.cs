﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.Masonry.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Registrars;

namespace Soenneker.Blazor.Masonry.Registrars;

/// <summary>
/// A Blazor interop library that integrates Masonry (https://masonry.desandro.com), the cascading grid layout library
/// </summary>
public static class MasonryRegistrar
{
    /// <summary>
    /// Shorthand for <code>services.TryAddScoped</code>
    /// </summary>
    public static void AddMasonry(this IServiceCollection services)
    {
        services.AddModuleImportUtil();
        services.TryAddScoped<IMasonryInterop, MasonryInterop>();
    }
}