namespace BlazorDeploy.Models;

public class Produto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public decimal Preco { get; set; }
    public string? Detalhe { get; set; }
    public string? Barcode { get; set; }
    public string? UrlImagem { get; set; }
    public string? CaminhoImagem => AppConfig.BaseUrl + UrlImagem;
    public bool Popular { get; set; }
    public bool MaisVendido { get; set; }
    public int EmEstoque { get; set; }
    public bool Disponivel { get; set; }
    public int CategoriaId { get; set; }
}
