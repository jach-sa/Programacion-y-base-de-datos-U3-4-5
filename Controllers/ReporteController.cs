using System.Collections.Generic;
using ChefRecetasCS.DAL;
using ChefRecetasCS.Models;

namespace ChefRecetasCS.Controllers
{
    public class ReporteController
    {
        private ReporteDAL dao =
            new ReporteDAL();

        public List<Receta> ObtenerReporte()
        {
            return dao.ObtenerReporteRecetas();
        }
    }
}