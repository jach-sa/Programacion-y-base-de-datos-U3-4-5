using ChefRecetasCS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using static System.ComponentModel.Design.ObjectSelectorEditor;


namespace ChefRecetasCS.DAL
{
    public class RecetaDAL
    {
        public List<Receta> Listar()
        {
            var lista = new List<Receta>();
            using var cn = Conexion.GetConexion();
            using var cmd = new MySqlCommand(
                "SELECT * FROM vw_recetas", cn);
            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read()) lista.Add(Mapear(dr));
            return lista;
        }

        public List<Receta> ListarPorCategoria(int idCat)
        {
            var lista = new List<Receta>();
            using var cn = Conexion.GetConexion();
            using var cmd = new MySqlCommand(
                "SELECT * FROM vw_recetas WHERE id_categoria=@id", cn);
            cmd.Parameters.AddWithValue("@id", idCat);
            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read()) lista.Add(Mapear(dr));
            return lista;
        }

        public void Insertar(Receta r)
        {
            using var cn = Conexion.GetConexion();
            using var cmd = new MySqlCommand(
                "INSERT INTO recetas(id_categoria,nombre,descripcion," +
                "tiempo_prep_min,porciones,dificultad,foto)" +
                " VALUES(@cat,@n,@d,@t,@p,@dif,@f)", cn);
            cmd.Parameters.AddWithValue("@cat", r.IdCategoria);
            cmd.Parameters.AddWithValue("@n", r.Nombre);
            cmd.Parameters.AddWithValue("@d", r.Descripcion);
            cmd.Parameters.AddWithValue("@t", r.TiempoPrepMin);
            cmd.Parameters.AddWithValue("@p", r.Porciones);
            cmd.Parameters.AddWithValue("@dif", r.Dificultad);
            cmd.Parameters.AddWithValue("@f",
                r.Foto ?? (object)DBNull.Value);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Actualizar(Receta r)
        {
            using var cn = Conexion.GetConexion();
            using var cmd = new MySqlCommand(
                "UPDATE recetas SET id_categoria=@cat,nombre=@n," +
                "descripcion=@d,tiempo_prep_min=@t,porciones=@p," +
                "dificultad=@dif,foto=@f WHERE id_receta=@id", cn);
            cmd.Parameters.AddWithValue("@cat", r.IdCategoria);
            cmd.Parameters.AddWithValue("@n", r.Nombre);
            cmd.Parameters.AddWithValue("@d", r.Descripcion);
            cmd.Parameters.AddWithValue("@t", r.TiempoPrepMin);
            cmd.Parameters.AddWithValue("@p", r.Porciones);
            cmd.Parameters.AddWithValue("@dif", r.Dificultad);
            cmd.Parameters.AddWithValue("@f",
                r.Foto ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", r.IdReceta);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var cn = Conexion.GetConexion();
            using var cmd = new MySqlCommand(
                "DELETE FROM recetas WHERE id_receta=@id", cn);
            cmd.Parameters.AddWithValue("@id", id);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        private Receta Mapear(MySqlDataReader dr)
        {
            return new Receta
            {
                IdReceta =
                    dr.GetInt32("id_receta"),

                IdCategoria =
                    dr.GetInt32("id_categoria"),

                Nombre =
                    dr.GetString("nombre"),

                Descripcion =
                    dr["descripcion"].ToString(),

                TiempoPrepMin =
                    dr.IsDBNull(
                        dr.GetOrdinal("tiempo_prep_min"))
                    ? 0
                    : dr.GetInt32("tiempo_prep_min"),

                Porciones =
                    dr.IsDBNull(
                        dr.GetOrdinal("porciones"))
                    ? 0
                    : dr.GetInt32("porciones"),

                Dificultad =
                    dr["dificultad"].ToString(),

                Foto =
                    dr.IsDBNull(
                        dr.GetOrdinal("foto"))
                    ? null
                    : (byte[])dr["foto"],

                Categoria =
                    dr["categoria"].ToString()
            };
        }

    }
}
