namespace BlazorDeploy.Models;

public class TodosPedidos
{
    public int Id { get; set; }
    public string? NomeCliente { get; set; }
    public int? ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataPedido { get; set; }
}

