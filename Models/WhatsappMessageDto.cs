namespace BlazorDeploy.Models;
public class WhatsappMessageDto
{
    public string Session { get; set; } = "cozinha"; // opcionalmente default
    public string Phone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
