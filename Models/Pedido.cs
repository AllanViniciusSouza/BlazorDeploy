namespace BlazorDeploy.Models;

public class Pedido
{
    public int Id { get; set; }
    public string? Endereco { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime? DataPedido { get; set; }
    public string? FormaPagamento { get; set; }

    // Primary payment method (e.g. Dinheiro, Credito, Debito, Pix, Ifood)
    public decimal? ValorPago1 { get; set; }

    // Optional secondary payment method
    public string? FormaPagamento2 { get; set; }
    public decimal? ValorPago2 { get; set; }

    // Status: Ex: "Finalizado", "Pendente", "A Caminho", "Retirada"
    public string? Status { get; set; }

    // New fields
    public string? ClienteNome { get; set; }
    public string? VendedorNome { get; set; }
    // Telefone do cliente (opcional)
    public string? Telefone { get; set; }

    public List<CartItem> Itens { get; set; } = new();
    // Data de pagamento quando a forma de pagamento for "A Prazo"
    public DateTime? DataPagamentoPrazo { get; set; }
    public DateTime? DataPagamentoPrazo2 { get; set; }

    // Observações livres do pedido
    public string? Observacoes { get; set; }
}
