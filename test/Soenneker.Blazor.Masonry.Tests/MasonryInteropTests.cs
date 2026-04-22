using Soenneker.Blazor.Masonry.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.Masonry.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class MasonryInteropTests : HostedUnitTest
{
    private readonly IMasonryInterop _util;

    public MasonryInteropTests(Host host) : base(host)
    {
        _util = Resolve<IMasonryInterop>(true);
    }

    [Test]
    public void Default()
    {

    }
}
