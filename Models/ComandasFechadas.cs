namespace BlazorDeploy.Models;

public class ComandasFechadas
{
    public int Id { get; set; }
    public int ComandaId { get; set; }
    public int? NumeroComanda { get; set; }
    public string? NomeCliente { get; set; }
    //public int? Mesa { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataPedido { get; set; }
    public string FormaPagamento { get; set; }
}
