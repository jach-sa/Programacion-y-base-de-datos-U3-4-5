using System.Collections.Generic;
using ChefRecetasCS.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ChefRecetasCS.Reports
{
    public class RptUsuarios : BaseReport
    {
        private readonly List<Usuario> datos;

        public RptUsuarios(
            List<Usuario> datos,
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
                    c.RelativeColumn(2);
                    c.RelativeColumn(1);
                });

                table.Header(header =>
                {
                    header.Cell().Text("Nombre").Bold();
                    header.Cell().Text("Usuario").Bold();
                    header.Cell().Text("Rol").Bold();
                    header.Cell().Text("Activo").Bold();
                });

                foreach (var u in datos)
                {
                    table.Cell().Text(u.Nombre);

                    table.Cell().Text(u.UserName);

                    table.Cell().Text(u.Rol);

                    table.Cell().Text(
                        u.Activo
                        ? "Sí"
                        : "No");
                }
            });
        }
    }
}