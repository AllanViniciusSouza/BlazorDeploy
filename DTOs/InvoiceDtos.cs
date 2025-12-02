using System;

namespace BlazorDeploy.DTOs
{
    public class InvoiceSummary
    {
        public string Id { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }

    public class InvoiceItem
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
