using System;
using System.Collections.Generic;
using System.Text;

namespace ChefRecetasCS.Models
{
   public class Categoria
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Icono { get; set; }

        public override string ToString() => Nombre;

    }
}
