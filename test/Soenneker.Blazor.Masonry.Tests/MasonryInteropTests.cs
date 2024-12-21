using Soenneker.Blazor.Masonry.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;


namespace Soenneker.Blazor.Masonry.Tests;

[Collection("Collection")]
public class MasonryInteropTests : FixturedUnitTest
{
    private readonly IMasonryInterop _util;

    public MasonryInteropTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IMasonryInterop>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
