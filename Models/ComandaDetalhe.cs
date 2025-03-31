    namespace BlazorDeploy.Models;

public class ComandaDetalhe
{
    public int Id { get; set; }
    public int Quantidade { get; set; }
    public decimal SubTotal { get; set; }
    public string? ProdutoNome { get; set; }
    public string? ProdutoImagem { get; set; }
    public string CaminhoImagem => AppConfig.BaseUrl + ProdutoImagem;
    public decimal ProdutoPreco { get; set; }
    public string NomeCliente { get; set; }
    public int ComandaId { get; set; }
    public DateTime DataAbertura { get; set; }
}
