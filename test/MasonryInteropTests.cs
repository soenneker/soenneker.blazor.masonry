using Soenneker.Blazor.Masonry.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Blazor.Masonry.Tests;

[Collection("Collection")]
public class MasonryInteropTests : FixturedUnitTest
{
    private readonly IMasonryInterop _interop;

    public MasonryInteropTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _interop = Resolve<IMasonryInterop>(true);
    }
}
