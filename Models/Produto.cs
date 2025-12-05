using System.Text.Json.Serialization;

namespace BlazorDeploy.Models;

public class Produto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    // legacy/default price - fallback
    public decimal Preco { get; set; }
    // new price fields introduced by backend
    public decimal PrecoCusto { get; set; }
    public decimal PrecoQuente { get; set; }
    public decimal PrecoGelada { get; set; }
    public decimal PrecoEntrega { get; set; }
    public decimal PrecoRetirar { get; set; }
    public string? Detalhe { get; set; }
    public string? Barcode { get; set; }
    public string? UrlImagem { get; set; }
    public string? CaminhoImagem => AppConfig.BaseUrl + UrlImagem;
    public bool Popular { get; set; }
    public bool MaisVendido { get; set; }
    public int EmEstoque { get; set; }
    public string? DiasDisponiveis { get; set; }
    public bool Disponivel { get; set; }
    public int CategoriaId { get; set; }
}
