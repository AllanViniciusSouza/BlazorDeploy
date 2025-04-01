namespace BlazorDeploy.Models;

public class Pedido
{
    public int Id { get; set; }
    public string? Endereco { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime? DataPedido { get; set; }
    public string? FormaPagamento { get; set; }
}
