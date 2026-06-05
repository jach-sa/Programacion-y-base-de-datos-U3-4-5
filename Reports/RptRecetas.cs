using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using ChefRecetasCS.Models;

namespace ChefRecetasCS.Reports
{
    public class RptRecetas : BaseReport
    {
        private readonly List<Receta> datos;

        public RptRecetas(
            List<Receta> datos,
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
                    c.RelativeColumn(3);
                    c.RelativeColumn(2);
                    c.RelativeColumn();
                    c.RelativeColumn();
                });

                table.Header(h =>
                {
                    h.Cell().Text("Nombre");
                    h.Cell().Text("Categoría");
                    h.Cell().Text("Tiempo");
                    h.Cell().Text("Dificultad");
                });

                foreach (var r in datos)
                {
                    table.Cell().Text(r.Nombre);
                    table.Cell().Text(r.Categoria);
                    table.Cell().Text(
                        r.TiempoPrepMin.ToString());

                    table.Cell().Text(
                        r.Dificultad);
                }
            });
        }
    }
}