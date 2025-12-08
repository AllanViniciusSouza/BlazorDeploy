namespace BlazorDeploy.Models
{
    public class CartItem
    {
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }   // <- preço editável no carrinho
        // Temperatura escolhida pelo cliente (null / "Quente" / "Gelada")
        public string? Temperatura { get; set; }

        // Marca se já foi aplicado acréscimo de entrega (ex.: +0,10)
        public bool DeliverySurchargeApplied { get; set; } = false;

        public decimal Total => PrecoUnitario * Quantidade;
    }
}
