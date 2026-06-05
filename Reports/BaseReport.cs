using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ChefRecetasCS.Reports
{
    public abstract class BaseReport : IDocument
    {
        protected string Usuario;
        protected string Titulo;

        protected BaseReport(
            string usuario,
            string titulo)
        {
            Usuario = usuario;
            Titulo = titulo;
        }

        public DocumentMetadata GetMetadata()
        {
            return DocumentMetadata.Default;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);

                page.Header().Column(col =>
                {
                    col.Item().Text("Chef Recetas")
                        .FontSize(20)
                        .Bold();

                    col.Item().Text("Instituto Tecnológico");

                    col.Item().Text(Titulo);

                    col.Item().Text(
                        $"Generado por: {Usuario}");

                    col.Item().Text(
                        $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                page.Content()
                    .Element(ComposeContent);

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
            });
        }

        protected abstract void ComposeContent(
            IContainer container);
    }
}