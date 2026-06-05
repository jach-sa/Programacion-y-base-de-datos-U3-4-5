using System.Collections.Generic;
using ChefRecetasCS.DAL;
using ChefRecetasCS.Models;

namespace ChefRecetasCS.Controllers
{
    public class UsuarioController
    {
        private UsuarioDAL dao =
            new UsuarioDAL();

        public Usuario Login(
            string usuario,
            string password)
        {
            return dao.Autenticar(
                usuario,
                password);
        }

        public void Registrar(
            string nombre,
            string usuario,
            string password,
            string rol,
            bool activo)
        {
            dao.Registrar(
                nombre,
                usuario,
                password,
                rol,
                activo);
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return dao.ObtenerUsuarios();
        }
        public int ObtenerTotalUsuarios()
        {
            return dao.ObtenerTotal();
        }
        public void GuardarUsuario(
            int id,
            string nombre,
            string usuario,
            string password,
            string rol,
            bool activo)
        {
            dao.GuardarUsuario(
                id,
                nombre,
                usuario,
                password,
                rol,
                activo);
        }

        public void EliminarUsuario(int id)
        {
            dao.EliminarUsuario(id);
        }
    }
}