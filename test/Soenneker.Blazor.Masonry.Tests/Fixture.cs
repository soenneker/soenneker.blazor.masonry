using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Serilog;
using Soenneker.Blazor.Masonry.Registrars;
using Soenneker.Fixtures.Unit;
using Soenneker.Utils.Test;

namespace Soenneker.Blazor.Masonry.Tests;

public class Fixture : UnitFixture
{
    public override System.Threading.Tasks.Task InitializeAsync()
    {
        SetupIoC(Services);

        return base.InitializeAsync();
    }

    private static void SetupIoC(IServiceCollection services)
    {
        services.AddLogging(builder => { builder.AddSerilog(dispose: true); });

        IConfiguration config = TestUtil.BuildConfig();
        services.AddSingleton(config);
        
        services.AddMasonry();
    }
}