using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorDeploy;
using BlazorDeploy.Services;
using BlazorDeploy.Validations;
using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

try
{
    // Carrega appsettings.json de wwwroot
    using var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    try
    {
        using var responseStream = await http.GetStreamAsync("appconfig.json");
        var config = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(responseStream);
        var apiBaseUrl = builder.HostEnvironment.BaseAddress; // fallback default
        if (config != null && config.TryGetValue("ApiBaseUrl", out var configured)) apiBaseUrl = configured;

        // set optional BarcodeLookup API key into AppConfig
        if (config != null && config.TryGetValue("BarcodeLookupApiKey", out var barcodeKey))
        {
            BlazorDeploy.Models.AppConfig.BarcodeLookupApiKey = barcodeKey;
        }

        // Registra HttpClient com URL da API
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
    }
    catch (Exception ex)
    {
        // don't stop the app if appconfig.json can't be loaded (mobile/network issues)
        Console.WriteLine($"⚠️ Aviso: não foi possível carregar appconfig.json: {ex.Message}. Usando Host base como ApiBaseUrl.");
        // fallback to app host base address
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
    }

}
catch (Exception ex)
{
    // very defensive: log but continue with fallback
    Console.WriteLine($"❌ Erro inesperado ao inicializar configuração: {ex.Message}");
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
}

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ApiService>();
builder.Services.AddSingleton<IValidator, Validator>();
builder.Services.AddScoped<AuthService>();
//builder.Services.AddScoped<GooglePlacesService>();

builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();
