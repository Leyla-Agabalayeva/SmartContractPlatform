using EsignPlatform.BLL.DTOs.Contract;
using EsignPlatform.BLL.Services.Interfaces;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using EsignPlatform.DAL.Enums;


namespace EsignPlatform.BLL.Services.Implementations
{
    public class PdfService : IPdfService
    {
        // Brend rəngi (yaşıl)
        private const string Primary = "#15803d";
        private const string PrimaryDark = "#166534";
        private const string Muted = "#64748b";

        public byte[] GenerateContractPdf(ContractDetailDto c)
        {
            var hash = ComputeHash(c);

            var doc = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(11).FontColor("#1e293b"));

                    page.Header().Element(header =>
                    {
                        header.Column(col =>
                        {
                            col.Item().Text("SmartContract.az")
                                .FontSize(18).Bold().FontColor(Primary);
                            col.Item().Text("Elektron müqavilə sənədi")
                                .FontSize(9).FontColor(Muted);
                            col.Item().PaddingTop(6).LineHorizontal(1.5f).LineColor(Primary);
                        });
                    });

                    page.Content().PaddingVertical(16).Column(col =>
                    {
                        col.Spacing(14);

                        col.Item().Text(c.Title).FontSize(16).Bold().FontColor(PrimaryDark);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Şablon: {c.TemplateName}").FontColor(Muted);
                            row.RelativeItem().AlignRight().Text($"Status: {c.Status}").FontColor(Muted);
                        });
                        col.Item().Text($"Yaradılma tarixi: {c.CreatedAt:dd.MM.yyyy HH:mm}").FontSize(9).FontColor(Muted);

                        // Müqavilə məlumatları
                        col.Item().PaddingTop(6).Text("Müqavilə məlumatları").FontSize(13).Bold().FontColor(Primary);
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(2);
                                cols.RelativeColumn(3);
                            });
                            foreach (var f in c.Fields)
                            {
                                var val = c.FieldValues.TryGetValue(f.Key, out var v) ? v : "-";
                                table.Cell().Border(0.5f).BorderColor("#e2e8f0").Padding(6).Text(f.Label).SemiBold();
                                table.Cell().Border(0.5f).BorderColor("#e2e8f0").Padding(6).Text(string.IsNullOrWhiteSpace(val) ? "-" : val);
                            }
                        });

                        // Tərəflər
                        col.Item().PaddingTop(6).Text("Tərəflər").FontSize(13).Bold().FontColor(Primary);
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3);
                                cols.RelativeColumn(2);
                                cols.RelativeColumn(2);
                                cols.RelativeColumn(2);
                            });
                            table.Header(h =>
                            {
                                h.Cell().Background(Primary).Padding(6).Text("Ad / Şirkət").FontColor("#ffffff").SemiBold();
                                h.Cell().Background(Primary).Padding(6).Text("FIN / VÖEN").FontColor("#ffffff").SemiBold();
                                h.Cell().Background(Primary).Padding(6).Text("Rol").FontColor("#ffffff").SemiBold();
                                h.Cell().Background(Primary).Padding(6).Text("İmza").FontColor("#ffffff").SemiBold();
                            });
                            foreach (var p in c.Parties)
                            {
                                table.Cell().Border(0.5f).BorderColor("#e2e8f0").Padding(6).Text(p.DisplayName);
                                table.Cell().Border(0.5f).BorderColor("#e2e8f0").Padding(6).Text(p.FinOrVoen);
                                table.Cell().Border(0.5f).BorderColor("#e2e8f0").Padding(6).Text(p.Role.ToString());
                                table.Cell().Border(0.5f).BorderColor("#e2e8f0").Padding(6)
                                    .Text(p.Signed ? $"İmzalandı ({p.SignedAt:dd.MM.yyyy})" : "Gözləyir")
                                    .FontColor(p.Signed ? Primary : "#b45309");
                            }
                        });
                    });

                    page.Footer().Column(col =>
                    {
                        col.Item().LineHorizontal(0.5f).LineColor("#e2e8f0");
                        col.Item().PaddingTop(4).Row(row =>
                        {
                            row.RelativeItem().Text($"Sənəd hash (SHA256): {hash[..32]}...")
                                .FontSize(7).FontColor(Muted);
                            row.ConstantItem(60).AlignRight().Text(x =>
                            {
                                x.CurrentPageNumber().FontSize(8).FontColor(Muted);
                                x.Span(" / ").FontSize(8).FontColor(Muted);
                                x.TotalPages().FontSize(8).FontColor(Muted);
                            });
                        });
                    });
                });
            });

            return doc.GeneratePdf();
        }

        public static string ComputeHash(ContractDetailDto c)
        {
            var sb = new StringBuilder();
            sb.Append(c.Id).Append('|').Append(c.Title).Append('|').Append(c.Status);
            foreach (var kv in c.FieldValues.OrderBy(k => k.Key))
                sb.Append('|').Append(kv.Key).Append('=').Append(kv.Value);
            foreach (var p in c.Parties.OrderBy(p => p.Id))
                sb.Append('|').Append(p.FinOrVoen).Append(':').Append(p.Signed);

            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(sb.ToString()));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }

}
