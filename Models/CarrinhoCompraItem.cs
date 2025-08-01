﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlazorDeploy.Models;

public class CarrinhoCompraItem
{
    public int Id { get; set; }
    public decimal Preco { get; set; }
    public decimal ValorTotal { get; set; }
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

    public int ProdutoId { get; set; }
    public Produto Produto { get; set; }
    public string? ProdutoNome { get; set; }
    public string? UrlImagem { get; set; }
    public string? CaminhoImagem => AppConfig.BaseUrl + UrlImagem;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
