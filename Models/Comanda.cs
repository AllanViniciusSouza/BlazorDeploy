using System.Text.Json.Serialization;

namespace BlazorDeploy.Models;

public class Comanda
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public DateTime DataAbertura { get; set; }
    public int? UsuarioId { get; set; }
    public int? ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal? ValorRecebido { get; set; }
    public string? FormaPagamento { get; set; }
    [JsonIgnore]
    public List<ItemComanda>? Itens { get; set; }
}  
