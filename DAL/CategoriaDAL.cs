using System;
using System.Collections.Generic;
using System.Text;
using ChefRecetasCS.Models;
using MySql.Data.MySqlClient;


namespace ChefRecetasCS.DAL
{
    public class CategoriaDAL
    {
        public List<Categoria> Listar()
        {
            var lista = new List<Categoria>();
            using var cn = Conexion.GetConexion();
            using var cmd = new MySqlCommand(
                "SELECT * FROM vw_categorias ORDER BY nombre", cn);
            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
                lista.Add(new Categoria
                {
                    IdCategoria = dr.GetInt32("id_categoria"),
                    Nombre = dr.GetString("nombre"),
                    Descripcion = dr["descripcion"].ToString(),
                    Icono = dr["icono"].ToString()
                });
            return lista;
        }

        public List<Categoria> ObtenerPorFecha(
    DateTime inicio,
    DateTime fin)
        {
            var lista = new List<Categoria>();

            using var cn = Conexion.GetConexion();

            using var cmd = new MySqlCommand(
                @"SELECT *
          FROM vw_categorias
          WHERE fecha_creacion
          BETWEEN @i AND @f",
                cn);

            cmd.Parameters.AddWithValue("@i", inicio);
            cmd.Parameters.AddWithValue("@f", fin);

            cn.Open();

            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Categoria
                {
                    IdCategoria = dr.GetInt32("id_categoria"),
                    Nombre = dr["nombre"].ToString(),
                    Descripcion = dr["descripcion"].ToString(),
                    Icono = dr["icono"].ToString()
                });
            }

            return lista;
        }

        public List<Categoria> ObtenerPorNombre(
    string nombre)
        {
            var lista = new List<Categoria>();

            using var cn = Conexion.GetConexion();

            using var cmd = new MySqlCommand(
                @"SELECT *
          FROM vw_categorias
          WHERE nombre
          LIKE @n",
                cn);

            cmd.Parameters.AddWithValue(
                "@n",
                "%" + nombre + "%");

            cn.Open();

            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Categoria
                {
                    IdCategoria = dr.GetInt32("id_categoria"),
                    Nombre = dr["nombre"].ToString(),
                    Descripcion = dr["descripcion"].ToString(),
                    Icono = dr["icono"].ToString()
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
                    @"SELECT nombre,
                     1 total
              FROM vw_categorias",
                    cn);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            while (dr.Read())
            {
                datos.Add(
                    dr["nombre"].ToString(),
                    dr.GetInt32("total"));
            }

            return datos;
        }

        public Categoria ObtenerDetalle(
    int id)
        {
            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    @"SELECT *
              FROM vw_categorias
              WHERE id_categoria=@id",
                    cn);

            cmd.Parameters.AddWithValue(
                "@id", id);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            if (dr.Read())
            {
                return new Categoria
                {
                    IdCategoria =
                        dr.GetInt32("id_categoria"),

                    Nombre =
                        dr["nombre"].ToString(),

                    Descripcion =
                        dr["descripcion"].ToString(),

                    Icono =
                        dr["icono"].ToString()
                };
            }

            return null;
        }

        public void Insertar(Categoria c)
        {
            using var cn = Conexion.GetConexion();
            using var cmd = new MySqlCommand(
                "INSERT INTO categorias(nombre,descripcion,icono)" +
                " VALUES(@n,@d,@i)", cn);
            cmd.Parameters.AddWithValue("@n", c.Nombre);
            cmd.Parameters.AddWithValue("@d", c.Descripcion);
            cmd.Parameters.AddWithValue("@i", c.Icono);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(Categoria c)
        {
            using var cn = Conexion.GetConexion();
            using var cmd = new MySqlCommand(
                "UPDATE categorias SET nombre=@n,descripcion=@d," +
                "icono=@i WHERE id_categoria=@id", cn);
            cmd.Parameters.AddWithValue("@n", c.Nombre);
            cmd.Parameters.AddWithValue("@d", c.Descripcion);
            cmd.Parameters.AddWithValue("@i", c.Icono);
            cmd.Parameters.AddWithValue("@id", c.IdCategoria);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var cn = Conexion.GetConexion();
            using var cmd = new MySqlCommand(
                "DELETE FROM categorias WHERE id_categoria=@id", cn);
            cmd.Parameters.AddWithValue("@id", id);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

     
    }
}
