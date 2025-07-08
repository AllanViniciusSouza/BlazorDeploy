namespace BlazorDeploy.Models;

public class ProdutoCardapio
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public bool Selecionado { get; set; } = false;
}
