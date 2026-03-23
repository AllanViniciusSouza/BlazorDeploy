namespace BlazorDeploy.Models;

/// <summary>
/// Representa a relação entre um fardo e um produto individual.
/// Exemplo: Um fardo de cerveja (código 7891234567890) contém 24 unidades de cerveja individual (código 7891234567891).
/// </summary>
public class ProdutoFardo
{
    public int Id { get; set; }
    
    /// <summary>
    /// ID do produto que representa o FARDO (embalagem com múltiplas unidades)
    /// </summary>
    public int ProdutoFardoId { get; set; }
    
    /// <summary>
    /// Código de barras do FARDO
    /// </summary>
    public string? BarcodeFardo { get; set; }
    
    /// <summary>
    /// ID do produto INDIVIDUAL (unidade)
    /// </summary>
    public int ProdutoUnidadeId { get; set; }
    
    /// <summary>
    /// Código de barras da unidade INDIVIDUAL
    /// </summary>
    public string? BarcodeUnidade { get; set; }
    
    /// <summary>
    /// Quantidade de unidades individuais que vêm em um fardo
    /// Exemplo: 24 (um fardo de cerveja contém 24 latinhas)
    /// </summary>
    public int QuantidadePorFardo { get; set; }
    
    /// <summary>
    /// Nome descritivo para facilitar identificação
    /// Exemplo: "Fardo Brahma 350ml c/ 24 unidades"
    /// </summary>
    public string? Descricao { get; set; }
    
    /// <summary>
    /// Se está ativo (para desabilitar conversões antigas)
    /// </summary>
    public bool Ativo { get; set; } = true;
}
