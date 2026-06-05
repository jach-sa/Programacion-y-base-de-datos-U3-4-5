using ChefRecetasCS.Models;
using ChefRecetasCS.Reports;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ChefRecetasCS.Reports
{
    public class RptCategorias : BaseReport
    {
        private readonly List<Categoria> datos;

        public RptCategorias(
            List<Categoria> datos,
            string usuario,
            string titulo)
            : base(usuario, titulo)
        {
            this.datos = datos;
        }

        protected override void ComposeContent(
            IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(c =>
                {
                    c.RelativeColumn(2);
                    c.RelativeColumn(4);
                });

                table.Header(h =>
                {
                    h.Cell().Text("Nombre");
                    h.Cell().Text("Descripción");
                });

                foreach (var c in datos)
                {
                    table.Cell().Text(c.Nombre);
                    table.Cell().Text(c.Descripcion);
                }
            });
        }
    }
}
