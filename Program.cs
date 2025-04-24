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
    using var responseStream = await http.GetStreamAsync("appsettings.json");
    var config = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(responseStream);
    var apiBaseUrl = config?["ApiBaseUrl"] ?? throw new Exception("ApiBaseUrl not found");

    // Registra HttpClient com URL da API
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Erro ao carregar o appsettings.json: {ex.Message}");
    throw; // Pra interromper e mostrar erro
}


builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ApiService>();
builder.Services.AddSingleton<IValidator, Validator>();

builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();
