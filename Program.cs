using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorDeploy;
using BlazorDeploy.Services;
using BlazorDeploy.Validations;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://n2v55gk9-7066.brs.devtunnels.ms/") });
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ApiService>();
builder.Services.AddSingleton<IValidator, Validator>();

await builder.Build().RunAsync();
