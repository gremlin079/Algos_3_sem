using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Northwind.Client;
using Northwind.Client.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var baseUrl = builder.Configuration.GetSection("Api")["BaseUrl"] ?? "http://localhost:5000";

builder.Services.AddHttpClient<ProductApiClient>(client =>
{
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddSingleton<ClientApp>();

var host = builder.Build();

var app = host.Services.GetRequiredService<ClientApp>();
await app.RunAsync();

