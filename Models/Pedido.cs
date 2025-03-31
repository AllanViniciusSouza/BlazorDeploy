namespace BlazorDeploy.Models;

public class Pedido
{
    public string? Endereco { get; set; }
    public decimal ValorTotal { get; set; }
    public int UsuarioId { get; set; }
    public int ClienteId { get; set; }
    public string? FormaPagamento { get; set; }
}
