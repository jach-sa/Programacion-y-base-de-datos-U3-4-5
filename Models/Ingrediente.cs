using System;
using System.Collections.Generic;
using System.Text;

namespace ChefRecetasCS.Models
{
    public class Ingrediente
    {
        public int IdIngrediente { get; set; }
        public string Nombre { get; set; }
        public string UnidadMedida { get; set; }
        public decimal CaloriasPorU { get; set; }
        public byte[] Imagen { get; set; }

        public override string ToString() => Nombre;

    }
}
