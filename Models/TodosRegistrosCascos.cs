using System.Text.Json.Serialization;

namespace BlazorDeploy.Models;

public class TodosRegistrosCascos
{
    public int Id { get; set; }  // Campo Auto Increment no banco de dados
    public string NomeCliente { get; set; }
    public string Telefone { get; set; }
    public int Quantidade { get; set; }
    public string TipoCasco { get; set; }
    public DateTime? DataEmprestimo { get; set; } = DateTime.Now; // Padrão: data atual
    public DateTime? DataDevolucao { get; set; } = DateTime.Now;
    [JsonIgnore] // Ignora esta propriedade ao serializar para JSON
    public string TelefoneFormatado
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Telefone) || Telefone.Length < 10)
                return Telefone;

            return $"({Telefone.Substring(0, 2)}) {Telefone.Substring(2, 5)}-{Telefone.Substring(7)}";
        }
    }
}
