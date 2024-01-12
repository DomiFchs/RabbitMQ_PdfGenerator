using Model.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Document = QuestPDF.Fluent.Document;

namespace PDFGenerator.Services;

public class PdfService {
    public void Generate(List<ToDo> todos) {
        Document.Create(container => {
            container.Page(page => {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header().Text("Todos").SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                page.Content().PaddingVertical(1, Unit.Centimetre).Column(column => {
                    foreach (var todo in todos)
                        column.Item().Row(row => {
                            row.Spacing(5);
                            row.AutoItem().Text($"{todos.IndexOf(todo)+1}. ");
                            row.RelativeItem().Text(todo.Title);
                        });
                });
                page.Footer().AlignCenter().Text(x => {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                });
            });
        }).GeneratePdf($"/app/pdfs/Todos_{Guid.NewGuid()}.pdf");
    }
}