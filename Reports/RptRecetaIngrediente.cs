using System.Collections.Generic;
using ChefRecetasCS.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ChefRecetasCS.Reports
{
    public class RptRecetaIngrediente : BaseReport
    {
        private readonly List<RecetaIngrediente> datos;

        public RptRecetaIngrediente(
            List<RecetaIngrediente> datos,
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
                    c.RelativeColumn(3);
                    c.RelativeColumn(1);
                    c.RelativeColumn(1);
                });

                table.Header(header =>
                {
                    header.Cell().Text("Receta").Bold();
                    header.Cell().Text("Ingrediente").Bold();
                    header.Cell().Text("Cantidad").Bold();
                    header.Cell().Text("Unidad").Bold();
                });

                foreach (var r in datos)
                {
                    table.Cell().Text(r.Receta);

                    table.Cell().Text(
                        r.Ingrediente);

                    table.Cell().Text(
                        r.Cantidad.ToString());

                    table.Cell().Text(
                        r.Unidad);
                }
            });
        }
    }
}