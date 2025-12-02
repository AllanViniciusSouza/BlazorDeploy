namespace BlazorDeploy.Models
{
    public class CartItem
    {
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }   // <- preço editável no carrinho

        public decimal Total => PrecoUnitario * Quantidade;
    }
}
