using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChefRecetasCS.Models
{
    public class RecetaIngrediente
    {
        public int IdRI { get; set; }

        public int IdReceta { get; set; }

        public int IdIngrediente { get; set; }

        public decimal Cantidad { get; set; }

        public string Unidad { get; set; }

        public string Receta { get; set; }

        public string Ingrediente { get; set; }
    }
}