    namespace BlazorDeploy.Models;

public class ComandaDetalhe
{
    public int Id { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorTotal { get; set; }
    public string? ProdutoNome { get; set; }
    public string? ProdutoImagem { get; set; }
    public string CaminhoImagem => AppConfig.BaseUrl + ProdutoImagem;
    public decimal Preco { get; set; }
    public string IdComanda { get; set; }
    public string? FormaPagamento { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime DataPedido { get; set; }
}
