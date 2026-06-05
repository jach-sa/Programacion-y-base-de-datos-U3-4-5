using System;
using System.Collections.Generic;
using System.Text;

namespace ChefRecetasCS.Models
{
    public class PasoPreparacionReporte
    {
        public int IDPaso { get; set; }
        public int IDReceta { get; set; }

        public int NumeroPaso { get; set; }

        public string Instruccion { get; set; }

        public string Receta { get; set; }
    }
}