using System;
using System.Collections.Generic;
using System.Text;

namespace ChefRecetasCS.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string UserName { get; set; }
        public string Rol { get; set; }
        public bool Activo { get; set; }

    }
}
