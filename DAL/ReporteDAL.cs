using System;
using System.Collections.Generic;
using ChefRecetasCS.Models;
using MySql.Data.MySqlClient;

namespace ChefRecetasCS.DAL
{
    public class ReporteDAL
    {
        public List<Receta> ObtenerReporteRecetas()
        {
            var lista = new List<Receta>();

            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    @"SELECT *
                      FROM vw_recetas
                      ORDER BY nombre",
                    cn);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Receta
                {
                    IdReceta =
                        dr.GetInt32("id_receta"),

                    Nombre =
                        dr["nombre"].ToString(),

                    Categoria =
                        dr["categoria"].ToString(),

                    TiempoPrepMin =
                        dr.IsDBNull(
                            dr.GetOrdinal(
                                "tiempo_prep_min"))
                        ? 0
                        : dr.GetInt32(
                            "tiempo_prep_min"),

                    Porciones =
                        dr.IsDBNull(
                            dr.GetOrdinal(
                                "porciones"))
                        ? 0
                        : dr.GetInt32(
                            "porciones"),

                    Dificultad =
                        dr["dificultad"]
                            .ToString()
                });
            }

            return lista;
        }

        public List<Receta> ObtenerRecetasPorFecha(
            DateTime inicio,
            DateTime fin)
        {
            var lista =
                new List<Receta>();

            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    @"SELECT *
                      FROM vw_recetas
                      WHERE fecha_creacion
                      BETWEEN @i AND @f",
                    cn);

            cmd.Parameters.AddWithValue(
                "@i", inicio);

            cmd.Parameters.AddWithValue(
                "@f", fin);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Receta
                {
                    IdReceta =
                        dr.GetInt32("id_receta"),

                    Nombre =
                        dr["nombre"].ToString(),

                    Categoria =
                        dr["categoria"].ToString(),

                    TiempoPrepMin =
                        dr.GetInt32(
                            "tiempo_prep_min"),

                    Porciones =
                        dr.GetInt32(
                            "porciones"),

                    Dificultad =
                        dr["dificultad"]
                            .ToString()
                });
            }

            return lista;
        }

        public List<Receta> ObtenerRecetasPorNombre(
            string criterio)
        {
            var lista =
                new List<Receta>();

            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    @"SELECT *
                      FROM vw_recetas
                      WHERE nombre
                      LIKE @c",
                    cn);

            cmd.Parameters.AddWithValue(
                "@c",
                "%" + criterio + "%");

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Receta
                {
                    IdReceta =
                        dr.GetInt32("id_receta"),

                    Nombre =
                        dr["nombre"].ToString(),

                    Categoria =
                        dr["categoria"].ToString(),

                    TiempoPrepMin =
                        dr.GetInt32(
                            "tiempo_prep_min"),

                    Porciones =
                        dr.GetInt32(
                            "porciones"),

                    Dificultad =
                        dr["dificultad"]
                            .ToString()
                });
            }

            return lista;
        }

        public Dictionary<string, int>
            ObtenerEstadisticaRecetas()
        {
            var datos =
                new Dictionary<string, int>();

            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    @"SELECT categoria,
                             COUNT(*) total
                      FROM vw_recetas
                      GROUP BY categoria",
                    cn);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            while (dr.Read())
            {
                datos.Add(
                    dr["categoria"].ToString(),
                    dr.GetInt32("total"));
            }

            return datos;
        }

        public Receta ObtenerDetalleReceta(
            int id)
        {
            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    @"SELECT *
                      FROM vw_recetas
                      WHERE id_receta=@id",
                    cn);

            cmd.Parameters.AddWithValue(
                "@id", id);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            if (dr.Read())
            {
                return new Receta
                {
                    IdReceta =
                        dr.GetInt32("id_receta"),

                    Nombre =
                        dr["nombre"].ToString(),

                    Categoria =
                        dr["categoria"].ToString(),

                    Descripcion =
                        dr["descripcion"]
                            .ToString(),

                    TiempoPrepMin =
                        dr.GetInt32(
                            "tiempo_prep_min"),

                    Porciones =
                        dr.GetInt32(
                            "porciones"),

                    Dificultad =
                        dr["dificultad"].ToString()
                };
            }

            return null;
        }
    }
}