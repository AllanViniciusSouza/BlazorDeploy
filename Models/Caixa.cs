namespace BlazorDeploy.Models;

public class Caixa
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public DateTime DataAbertura { get; set; }
    public decimal ValorAbertura { get; set; }
    public DateTime? DataFechamento { get; set; }
    public decimal? ValorFechamento { get; set; }
    public string? Observacao { get; set; }
}
