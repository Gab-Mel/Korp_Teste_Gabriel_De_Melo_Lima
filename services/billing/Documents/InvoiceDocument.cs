using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using billing.Entities;
using System.Globalization;

namespace billing.Documents
{
    public class InvoiceDocument : IDocument
    {
        private readonly Invoice _invoice;

        public InvoiceDocument(Invoice invoice)
        {
            _invoice = invoice;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5.Landscape());
                page.Margin(40);

                page.DefaultTextStyle(x => x.FontSize(12));

                // 🔹 HEADER
                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("INVOICE")
                            .FontSize(24)
                            .Bold();

                        col.Item().Text($"# {_invoice.Number}")
                            .FontSize(14)
                            .SemiBold();
                    });

                    row.ConstantItem(200).AlignRight().Column(col =>
                    {
                        col.Item().Text($"Status: {_invoice.Status}")
                            .Bold()
                            .FontColor(_invoice.Status == "OPEN" ? Colors.Orange.Medium : Colors.Green.Medium);

                        col.Item().Text($"Data: {_invoice.CreatedAt:dd/MM/yyyy}");
                    });
                });

                // 🔹 CUSTOMER INFO
                page.Content().Column(col =>
                {
                    col.Spacing(15);

                    col.Item().LineHorizontal(1);

                    col.Item().Column(info =>
                    {
                        info.Item().Text("Cliente")
                            .Bold()
                            .FontSize(14);

                        info.Item().Text(_invoice.CustomerName);
                    });

                    col.Item().LineHorizontal(1);

                    // 🔹 TABELA
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4); // descrição
                            columns.RelativeColumn(2); // preço
                            columns.RelativeColumn(1); // qtd
                            columns.RelativeColumn(2); // total
                        });

                        // HEADER
                        table.Header(header =>
                        {
                            header.Cell().Text("Produto").Bold();
                            header.Cell().AlignRight().Text("Preço").Bold();
                            header.Cell().AlignCenter().Text("Qtd").Bold();
                            header.Cell().AlignRight().Text("Total").Bold();

                            header.Cell().ColumnSpan(4).PaddingVertical(5).LineHorizontal(1);
                        });

                        // ROWS
                        foreach (var item in _invoice.Items)
                        {
                            table.Cell().Text(item.Description);

                            table.Cell().AlignRight().Text(FormatCurrency(item.UnitPrice));

                            table.Cell().AlignCenter().Text(item.Quantity.ToString());

                            table.Cell().AlignRight().Text(FormatCurrency(item.Total));
                        }
                    });

                    // 🔹 TOTAL
                    col.Item().AlignRight().Column(total =>
                    {
                        total.Item().LineHorizontal(1);

                        total.Item().Text($"TOTAL: {FormatCurrency(_invoice.Total)}")
                            .FontSize(16)
                            .Bold();
                    });
                });

                // 🔹 FOOTER
                page.Footer()
                    .AlignCenter()
                    .Text("Gerado automaticamente pelo sistema")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Darken1);
            });
        }

        private string FormatCurrency(decimal value)
        {
            return value.ToString("C", new CultureInfo("pt-BR"));
        }
    }
}