using System.Collections.Generic;
using ChefRecetasCS.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ChefRecetasCS.Reports
{
    public class RptPasosPreparacion : BaseReport
    {
        private readonly List<PasoPreparacionReporte> datos;

        public RptPasosPreparacion(
            List<PasoPreparacionReporte> datos,
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
                    c.RelativeColumn(1);
                    c.RelativeColumn(6);
                });

                table.Header(header =>
                {
                    header.Cell().Text("Receta").Bold();
                    header.Cell().Text("Paso").Bold();
                    header.Cell().Text("Instrucción").Bold();
                });

                foreach (var p in datos)
                {
                    table.Cell().Text(
                        p.Receta);

                    table.Cell().Text(
                        p.NumeroPaso.ToString());

                    table.Cell().Text(
                        p.Instruccion);
                }
            });
        }
    }
}