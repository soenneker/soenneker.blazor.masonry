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
    /// Preloads required scripts and resources for Masonry.
    /// </summary>
    ValueTask Warmup(bool useCdn = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initializes a Masonry instance. This should be called in the `OnAfterRenderAsync` override. <para/>
    /// Defaults to Bootstrap selectors (.container for container, and .row for itemSelector). <para/>
    /// Each instance requires a unique <paramref name="id"/> to avoid conflicts.
    /// </summary>
    ValueTask Init(string id, string containerSelector = ".container", string itemSelector = ".row", bool percentPosition = true,
        float transitionDurationSecs = .2F, bool useCdn = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Triggers a layout update for an existing Masonry instance.
    /// </summary>
    ValueTask Layout(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Destroys a Masonry instance and removes it from memory.
    /// </summary>
    ValueTask Destroy(string id, CancellationToken cancellationToken = default);
}