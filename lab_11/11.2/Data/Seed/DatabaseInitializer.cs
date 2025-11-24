using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Northwind.Api.Data.Seed;

public class DatabaseInitializer(IServiceScopeFactory scopeFactory, IWebHostEnvironment environment) : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly IWebHostEnvironment _environment = environment;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, "Data"));

        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<NorthwindContext>();

        await context.Database.EnsureCreatedAsync(cancellationToken);

        if (!await context.Products.AnyAsync(cancellationToken))
        {
            context.Products.AddRange(ProductSeedData.Records);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

