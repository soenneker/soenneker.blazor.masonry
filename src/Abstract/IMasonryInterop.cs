using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Masonry.Abstract;

/// <summary>
/// A Blazor interop library that integrates Masonry (https://masonry.desandro.com), the cascading grid layout library
/// </summary>
public interface IMasonryInterop : IAsyncDisposable
{
    /// <summary>
    /// Initialize Masonry within your Razor code in the `OnAfterRenderAsync` override. <para/>
    /// Defaults to bootstrap selectors (.container for container, and .row for itemSelector)
    /// </summary>
    ValueTask Init(string containerSelector = ".container", string itemSelector = ".row", bool percentPosition = true, float transitionDurationSecs = .2F, CancellationToken cancellationToken = default);
}