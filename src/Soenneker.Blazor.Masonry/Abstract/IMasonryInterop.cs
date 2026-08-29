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
    /// <param name="useCdn">Whether cdn.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when warmup is complete.</returns>
    ValueTask Warmup(bool useCdn = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initializes a Masonry instance. This should be called in the `OnAfterRenderAsync` override. <para/>
    /// Each instance requires a unique <paramref name="elementId"/> to avoid conflicts.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="containerSelector">Container Selector for the init operation.</param>
    /// <param name="itemSelector">item Selector to inspect or update.</param>
    /// <param name="columnWidthSelector">Column Width Selector for the init operation.</param>
    /// <param name="percentPosition">Whether percent position.</param>
    /// <param name="transitionDurationSecs">Transition Duration Secs for the init operation.</param>
    /// <param name="useCdn">Whether cdn.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the masonry is ready for use.</returns>
    public ValueTask Init(string elementId, string? containerSelector = null, string itemSelector = ".masonry-item", string? columnWidthSelector = null, bool percentPosition = true,
        float transitionDurationSecs = .2F, bool useCdn = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates observer.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the observer creation is complete.</returns>
    ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Triggers a layout update for an existing Masonry instance.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the layout operation is complete.</returns>
    ValueTask Layout(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Destroys a Masonry instance and removes it from memory.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the destroy operation is complete.</returns>
    ValueTask Destroy(string elementId, CancellationToken cancellationToken = default);
}
