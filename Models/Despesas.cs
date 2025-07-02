using System.ComponentModel.DataAnnotations;

namespace BlazorDeploy.Models
{
    public class Despesas
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataSelecionada { get; set; } = DateTime.Today;
        public string Descricao { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;

        public string FormaPagamento { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public int Parcelas{ get; set; }

        public string? Observação { get; set; } = string.Empty;
    }

}
