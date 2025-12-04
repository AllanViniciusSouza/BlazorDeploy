using static System.Net.WebRequestMethods;

namespace BlazorDeploy.Models;

public class AppConfig
{
    public static readonly string BaseUrl = "https://hc4df66l-7066.brs.devtunnels.ms/";
    public static readonly string PerfilImagemPadrao = "Resources/Images/user1.png";
    // BarcodeLookup API key (set from appconfig.json in Program.cs)
    public static string? BarcodeLookupApiKey { get; set; }
}
