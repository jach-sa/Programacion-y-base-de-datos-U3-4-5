using System.Collections.Generic;
using ChefRecetasCS.Models;
using MySql.Data.MySqlClient;

namespace ChefRecetasCS.DAL
{
    public class UsuarioDAL
    {
        // =====================================
        // LOGIN
        // =====================================

        public Usuario Autenticar(
            string usr,
            string pass)
        {
            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    "SELECT * FROM usuarios " +
                    "WHERE usuario=@u " +
                    "AND password=SHA2(@p,256)"+
                    "AND activo = 1",
                    cn);

            cmd.Parameters.AddWithValue("@u", usr);

            cmd.Parameters.AddWithValue("@p", pass);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            if (dr.Read())
            {
                return new Usuario
                {
                    IdUsuario =
                        dr.GetInt32("id_usuario"),

                    Nombre =
                        dr.GetString("nombre"),

                    UserName =
                        dr.GetString("usuario"),

                    Rol =
                        dr.GetString("rol"),

                    Activo = 
                        dr.GetBoolean("activo")
                };
            }

            return null;
        }

        // =====================================
        // REGISTRAR
        // =====================================

        public void Registrar(
    string nombre,
    string usuario,
    string password,
    string rol,
    bool activo)
        {
            using var cn = Conexion.GetConexion();

            using var cmd = new MySqlCommand(
                "INSERT INTO usuarios(nombre,usuario,password,rol,activo) " +
                "VALUES(@n,@u,SHA2(@p,256),@r,@a)", cn);

            cmd.Parameters.AddWithValue("@n", nombre);
            cmd.Parameters.AddWithValue("@u", usuario);
            cmd.Parameters.AddWithValue("@p", password);
            cmd.Parameters.AddWithValue("@r", rol);
            cmd.Parameters.AddWithValue("@a", activo);

            cn.Open();

            cmd.ExecuteNonQuery();
        }

        // =====================================
        // LISTAR
        // =====================================

        public List<Usuario> ObtenerUsuarios()
        {
            var lista = new List<Usuario>();

            using var cn = Conexion.GetConexion();

            using var cmd = new MySqlCommand(
                "SELECT * FROM vw_usuarios ORDER BY nombre", cn);

            cn.Open();

            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = dr.GetInt32("id_usuario"),
                    Nombre = dr.GetString("nombre"),
                    UserName = dr.GetString("usuario"),
                    Rol = dr.GetString("rol"),
                    Activo = dr.GetBoolean("activo")
                });
            }

            return lista;
        }

        public List<Usuario> ObtenerPorFecha(
    DateTime inicio,
    DateTime fin)
        {
            var lista = new List<Usuario>();

            using var cn = Conexion.GetConexion();

            using var cmd = new MySqlCommand(
                @"SELECT *
          FROM vw_usuarios
          WHERE fecha_creacion
          BETWEEN @i AND @f
          ORDER BY fecha_creacion",
                cn);

            cmd.Parameters.AddWithValue("@i", inicio);
            cmd.Parameters.AddWithValue("@f", fin);

            cn.Open();

            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = dr.GetInt32("id_usuario"),
                    Nombre = dr.GetString("nombre"),
                    UserName = dr.GetString("usuario"),
                    Rol = dr.GetString("rol"),
                    Activo = dr.GetBoolean("activo")
                });
            }

            return lista;
        }

        public List<Usuario> ObtenerPorNombre(
    string criterio)
        {
            var lista = new List<Usuario>();

            using var cn = Conexion.GetConexion();

            using var cmd = new MySqlCommand(
                @"SELECT *
          FROM vw_usuarios
          WHERE nombre LIKE @c
          ORDER BY nombre",
                cn);

            cmd.Parameters.AddWithValue(
                "@c",
                "%" + criterio + "%");

            cn.Open();

            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = dr.GetInt32("id_usuario"),
                    Nombre = dr.GetString("nombre"),
                    UserName = dr.GetString("usuario"),
                    Rol = dr.GetString("rol"),
                    Activo = dr.GetBoolean("activo")
                });
            }

            return lista;
        }

        public List<Usuario> ObtenerPorRol(
    string rol)
        {
            var lista = new List<Usuario>();

            using var cn = Conexion.GetConexion();

            using var cmd = new MySqlCommand(
                @"SELECT *
          FROM vw_usuarios
          WHERE rol=@r
          ORDER BY nombre",
                cn);

            cmd.Parameters.AddWithValue("@r", rol);

            cn.Open();

            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = dr.GetInt32("id_usuario"),
                    Nombre = dr.GetString("nombre"),
                    UserName = dr.GetString("usuario"),
                    Rol = dr.GetString("rol"),
                    Activo = dr.GetBoolean("activo")
                });
            }

            return lista;
        }

        public Dictionary<string, int>
    ObtenerEstadisticas()
        {
            var datos =
                new Dictionary<string, int>();

            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    @"SELECT rol,
                     COUNT(*) total
              FROM vw_usuarios
              GROUP BY rol",
                    cn);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            while (dr.Read())
            {
                datos.Add(
                    dr["rol"].ToString(),
                    dr.GetInt32("total"));
            }

            return datos;
        }

        public Usuario ObtenerDetalle(
    int id)
        {
            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    @"SELECT *
              FROM vw_usuarios
              WHERE id_usuario=@id",
                    cn);

            cmd.Parameters.AddWithValue(
                "@id",
                id);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            if (dr.Read())
            {
                return new Usuario
                {
                    IdUsuario =
                        dr.GetInt32("id_usuario"),

                    Nombre =
                        dr.GetString("nombre"),

                    UserName =
                        dr.GetString("usuario"),

                    Rol =
                        dr.GetString("rol"),

                    Activo =
                        dr.GetBoolean("activo")
                };
            }

            return null;
        }

        public int ObtenerTotal()
        {
            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    @"SELECT COUNT(*)
              FROM usuarios",
                    cn);

            cn.Open();

            return Convert.ToInt32(
                cmd.ExecuteScalar());
        }

        // =====================================
        // ACTUALIZAR
        // =====================================

        public void ActualizarUsuario(
            int id,
            string nombre,
            string usuario,
            string password,
            string rol)
        {
            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    "UPDATE usuarios SET " +
                    "nombre=@n, " +
                    "usuario=@u, " +
                    "password=SHA2(@p,256), " +
                    "rol=@r " +
                    "WHERE id_usuario=@id",
                    cn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.Parameters.AddWithValue("@n", nombre);

            cmd.Parameters.AddWithValue("@u", usuario);

            cmd.Parameters.AddWithValue("@p", password);

            cmd.Parameters.AddWithValue("@r", rol);

            cn.Open();

            cmd.ExecuteNonQuery();
        }

        // =====================================
        // ELIMINAR
        // =====================================

        public void EliminarUsuario(int id)
        {
            using var cn = Conexion.GetConexion();

            using var cmd = new MySqlCommand(
                "DELETE FROM usuarios WHERE id_usuario=@id", cn);

            cmd.Parameters.AddWithValue("@id", id);

            cn.Open();

            cmd.ExecuteNonQuery();
        }

        public void GuardarUsuario(
    int id,
    string nombre,
    string usuario,
    string password,
    string rol,
    bool activo)
        {
            using var cn = Conexion.GetConexion();

            MySqlCommand cmd;

            if (id == 0)
            {
                cmd = new MySqlCommand(
                    "INSERT INTO usuarios(nombre,usuario,password,rol,activo) " +
                    "VALUES(@n,@u,SHA2(@p,256),@r,@a)", cn);

                cmd.Parameters.AddWithValue("@p", password);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    cmd = new MySqlCommand(
                        "UPDATE usuarios SET " +
                        "nombre=@n, usuario=@u, rol=@r, activo=@a " +
                        "WHERE id_usuario=@id", cn);
                }
                else
                {
                    cmd = new MySqlCommand(
                        "UPDATE usuarios SET " +
                        "nombre=@n, usuario=@u, " +
                        "password=SHA2(@p,256), " +
                        "rol=@r, activo=@a " +
                        "WHERE id_usuario=@id", cn);

                    cmd.Parameters.AddWithValue("@p", password);
                }

                cmd.Parameters.AddWithValue("@id", id);
            }

            cmd.Parameters.AddWithValue("@n", nombre);
            cmd.Parameters.AddWithValue("@u", usuario);
            cmd.Parameters.AddWithValue("@r", rol);
            cmd.Parameters.AddWithValue("@a", activo);

            cn.Open();

            cmd.ExecuteNonQuery();
        }
    }
}
