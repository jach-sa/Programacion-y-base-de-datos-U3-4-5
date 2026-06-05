using System;
using System.Collections.Generic;
using System.Text;
using ChefRecetasCS.DAL;
using ChefRecetasCS.Models;

namespace ChefRecetasCS.Controllers
{
    public class CategoriaController
    {
        private readonly CategoriaDAL _dal = new CategoriaDAL();

        public List<Categoria> ObtenerTodas() => _dal.Listar();

        public void Guardar(Categoria c)
        {
            if (c.IdCategoria == 0) _dal.Insertar(c);
            else _dal.Actualizar(c);
        }

        public void Eliminar(int id) => _dal.Eliminar(id);
    }
}
