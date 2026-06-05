using System;
using System.Collections.Generic;
using ChefRecetasCS.Models;
using MySql.Data.MySqlClient;

namespace ChefRecetasCS.DAL
{
    public class PasoPreparacionDAL
    {
        public List<PasoPreparacionReporte>
            ObtenerTodos()
        {
            var lista =
                new List<PasoPreparacionReporte>();

            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    "SELECT * FROM vw_pasos_preparacion",
                    cn);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(Mapear(dr));
            }

            return lista;
        }

        public List<PasoPreparacionReporte>
            ObtenerPorReceta(
                string criterio)
        {
            var lista =
                new List<PasoPreparacionReporte>();

            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    @"SELECT *
                      FROM vw_pasos_preparacion
                      WHERE receta LIKE @c",
                    cn);

            cmd.Parameters.AddWithValue(
                "@c",
                "%" + criterio + "%");

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(Mapear(dr));
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
                    @"SELECT receta,
                             COUNT(*) total
                      FROM vw_pasos_preparacion
                      GROUP BY receta",
                    cn);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            while (dr.Read())
            {
                datos.Add(
                    dr["receta"].ToString(),
                    dr.GetInt32("total"));
            }

            return datos;
        }

        public PasoPreparacionReporte
            ObtenerDetalle(
                int id)
        {
            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    @"SELECT *
                      FROM vw_pasos_preparacion
                      WHERE id_paso=@id",
                    cn);

            cmd.Parameters.AddWithValue(
                "@id",
                id);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            if (dr.Read())
            {
                return Mapear(dr);
            }

            return null;
        }

        private PasoPreparacionReporte Mapear(
            MySqlDataReader dr)
        {
            return new PasoPreparacionReporte
            {
                IDPaso =
                    dr.GetInt32("id_paso"),

                IDReceta =
                    dr.GetInt32("id_receta"),

                NumeroPaso =
                    dr.GetInt32("numero_paso"),

                Instruccion =
                    dr["instruccion"].ToString(),

                Receta =
                    dr["receta"].ToString()
            };
        }
    }
}