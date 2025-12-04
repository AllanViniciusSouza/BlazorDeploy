namespace BlazorDeploy.Models;

public class ImpressaoResultado
{
    public int PedidoId { get; set; }
    public string? Texto { get; set; }
    public List<PedidoDetalhe>? Itens { get; set; }
}
