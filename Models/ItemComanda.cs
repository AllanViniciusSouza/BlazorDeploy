using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace BlazorDeploy.Models;

public class ItemComanda : INotifyPropertyChanged
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal PrecoUnitario { get; set; }
    [JsonIgnore]
    public decimal ValorTotal => PrecoUnitario * Quantidade;
    [JsonIgnore]
    private int quantidade;

    public int Quantidade
    {
        get { return quantidade; }
        set
        {
            if (quantidade != value)
            {
                quantidade = value;
                OnPropertyChanged();
            }
        }
    }
    public int? ComandaId { get; set; }
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; }
    [JsonIgnore]
    public string? UrlImagem { get; set; }
    [JsonIgnore]
    public string? CaminhoImagem => AppConfig.BaseUrl + UrlImagem;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}
