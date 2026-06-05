using System.Threading;
using System.Threading.Tasks;
using Soenneker.Lepton.Suite.Abstract;

namespace Soenneker.Blazor.Masonry.Abstract;

/// <summary>
/// Defines the contract for a Masonry Blazor component interop wrapper.
/// </summary>
public interface IMasonry : ILeptonIdentifiableContentElement
{
    /// <summary>
    /// Gets or sets a value indicating whether the component should automatically initialize Masonry on first render.
    /// </summary>
    bool AutoRender { get; set; }

    /// <summary>
    /// Initializes the Masonry instance and sets up the observer.
    /// This should be called when <see cref="AutoRender"/> is false or when re-initializing.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    ValueTask Init(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the layout operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask Layout(CancellationToken cancellationToken = default);
}
