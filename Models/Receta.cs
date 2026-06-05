using System;
using System.Collections.Generic;
using System.Text;

namespace ChefRecetasCS.Models
{
    public class Receta
    {
        public int IdReceta { get; set; }
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int TiempoPrepMin { get; set; }
        public int Porciones { get; set; }
        public string Dificultad { get; set; }
        public byte[] Foto { get; set; }
        public string Categoria { get; set; }

        public override string ToString() => Nombre;

    }
}
