using BlazorDeploy.Models;

public class Estoque
{
    public int Id { get; set; } // Identificador único do estoque
    public int ProdutoId { get; set; } // Referência ao produto
    public Produto Produto { get; set; } = new(); // Navegação para a entidade Produto
    public int Quantidade { get; set; } // Quantidade disponível no estoque
    public DateTime DataEntrada { get; set; } // Data de entrada do produto no estoque
    public DateTime? DataValidade { get; set; } // Data de validade (se aplicável)
    public decimal? PrecoCusto { get; set; } // Preço de custo do produto no estoque
    public string? LocalArmazenamento { get; set; } // Local onde o produto está armazenado
    public bool Ativo { get; set; } = true; // Indica se o item está ativo no estoque
}
