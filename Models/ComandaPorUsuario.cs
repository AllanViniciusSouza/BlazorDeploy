namespace BlazorDeploy.Models;

public class ComandaPorUsuario
{
    public int Id { get; set; }
    public string? NomeCliente { get; set; }
    public int? Mesa { get; set; }
    public decimal PedidoTotal { get; set; }
    public DateTime DataComanda { get; set; }
}
