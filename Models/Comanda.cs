namespace BlazorDeploy.Models;

public class Comanda
{
    public int Id { get; set; }
    public DateTime DataAbertura { get; set; }
    public string IdComanda { get; set; }
    public int UsuarioId { get; set; }
    public string Status { get; set; } // Padrão: "Aberta"
    public decimal ValorTotal { get; set; }
    public decimal ValorRecebido { get; set; }
    public string? FormaPagamento { get; set; }
    public List<ComandaItem> Itens { get; set; }
}  
