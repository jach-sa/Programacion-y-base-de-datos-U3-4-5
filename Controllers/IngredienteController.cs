using System;
using System.Collections.Generic;
using System.Text;
using ChefRecetasCS.DAL;
using ChefRecetasCS.Models;


namespace ChefRecetasCS.Controllers
{
    public class IngredienteController
    {
        private IngredienteDAL dao = new IngredienteDAL();

        public List<Ingrediente> ObtenerIngredientes()
        {
            return dao.Listar();
        }

        public void GuardarIngrediente(Ingrediente ingrediente)
        {
            if (ingrediente.IdIngrediente == 0)
                dao.Insertar(ingrediente);
            else
                dao.Actualizar(ingrediente);
        }

        public void EliminarIngrediente(int id)
        {
            dao.Eliminar(id);
        }
    }
}
