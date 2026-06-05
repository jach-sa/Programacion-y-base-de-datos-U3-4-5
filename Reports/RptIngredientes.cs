using System.Collections.Generic;
using ChefRecetasCS.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ChefRecetasCS.Reports
{
    public class RptIngredientes : BaseReport
    {
        private readonly List<Ingrediente> datos;

        public RptIngredientes(
            List<Ingrediente> datos,
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
                    c.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    header.Cell().Text("Ingrediente").Bold();
                    header.Cell().Text("Unidad").Bold();
                    header.Cell().Text("Calorías").Bold();
                });

                foreach (var i in datos)
                {
                    table.Cell().Text(i.Nombre);
                    table.Cell().Text(i.UnidadMedida);
                    table.Cell().Text(
                        i.CaloriasPorU.ToString("N2"));
                }
            });
        }
    }
}