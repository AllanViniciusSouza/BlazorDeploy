using System;
using System.Collections.Generic;

namespace BlazorDeploy.DTOs
{
    public class NotaEntradaDto
    {
        public int Id { get; set; }
        public string NumeroNota { get; set; } = string.Empty;
        public DateTime DataEmissao { get; set; }
        public string? Fornecedor { get; set; }
        public decimal ValorTotal { get; set; }
        public List<NotaEntradaItemDto>? Itens { get; set; }
    }

    public class NotaEntradaItemDto
    {
        public int Id { get; set; }
        public int NotaEntradaId { get; set; }
        public int? ProdutoId { get; set; }
        public string? ProdutoNome { get; set; }
        public string? Nome { get; set; }
        public string? Barcode { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoCusto { get; set; }
    }
}
