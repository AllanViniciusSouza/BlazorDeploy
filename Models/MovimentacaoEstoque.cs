using System.Text.Json.Serialization;

namespace BlazorDeploy.Models;

public class MovimentacaoEstoque
{
    public int Id { get; set; } // Chave primária
    public int ProdutoId { get; set; } // Chave estrangeira para Produto
    public int Quantidade { get; set; } // Quantidade movimentada
    public TipoMovimentacao Tipo { get; set; } // Entrada ou Saída
    public DateTime DataMovimentacao { get; set; } = DateTime.UtcNow; // Data do movimento
    public string? Observacao { get; set; } // Comentário opcional sobre a movimentação
    // Relacionamento com Produto
    public Produto Produto { get; set; }
}
