using System;
using System.Collections.Generic;
using System.Text;
using ChefRecetasCS.DAL;
using ChefRecetasCS.Models;


namespace ChefRecetasCS.Controllers
{
    public class RecetaController
    {
        private readonly RecetaDAL _dal = new RecetaDAL();

        public List<Receta> ObtenerTodas() => _dal.Listar();

        public List<Receta> ObtenerPorCategoria(int idCat)
            => _dal.ListarPorCategoria(idCat);

        public void Guardar(Receta r)
        {
            if (r.IdReceta == 0) _dal.Insertar(r);
            else _dal.Actualizar(r);
        }

        public void Eliminar(int id) => _dal.Eliminar(id);

        public List<Receta> Buscar(string texto)
        {
            texto = texto.ToLower();

            return _dal.Listar().FindAll(r => r.Nombre.ToLower().Contains(texto)

                    ||

                    r.Descripcion.ToLower().Contains(texto));
        }
    }
}
