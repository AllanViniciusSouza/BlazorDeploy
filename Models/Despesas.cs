using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BlazorDeploy.Shared;

namespace BlazorDeploy.Models
{
    public class Despesas
    {
        public int Id { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("dataSelecionada")]
        public DateTime Data { get; set; } = DateTime.Today;
        public string Descricao { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;

        public string FormaPagamento { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public int Parcelas{ get; set; }

        public string? Observacao { get; set; } = string.Empty;
        public int UsuarioId { get; set; } = 1;
    }

}
