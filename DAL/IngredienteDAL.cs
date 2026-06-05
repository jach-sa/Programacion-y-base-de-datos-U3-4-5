using System;
using System.Collections.Generic;
using System.Text;
using ChefRecetasCS.Models;
using MySql.Data.MySqlClient;


namespace ChefRecetasCS.DAL
{
    public class IngredienteDAL
    {
        public List<Ingrediente> Listar()
        {
            var lista = new List<Ingrediente>();
            using var cn = Conexion.GetConexion();
            using var cmd = new MySqlCommand(
                "SELECT * FROM vw_ingredientes ORDER BY nombre", cn);
            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
                lista.Add(new Ingrediente
                {
                    IdIngrediente = dr.GetInt32("id_ingrediente"),
                    Nombre = dr.GetString("nombre"),
                    UnidadMedida = dr["unidad_medida"].ToString(),
                    CaloriasPorU = dr.IsDBNull(
                        dr.GetOrdinal("calorias_por_u")) ? 0
                        : dr.GetDecimal("calorias_por_u"),
                    Imagen = dr.IsDBNull(dr.GetOrdinal("imagen"))
                        ? null : (byte[])dr["imagen"]
                });
            return lista;
        }
        public List<Ingrediente> ObtenerPorFecha(
    DateTime inicio,
    DateTime fin)
        {
            var lista = new List<Ingrediente>();

            using var cn = Conexion.GetConexion();

            using var cmd = new MySqlCommand(
                @"SELECT *
          FROM vw_ingredientes
          WHERE fecha_creacion
          BETWEEN @i AND @f",
                cn);

            cmd.Parameters.AddWithValue("@i", inicio);
            cmd.Parameters.AddWithValue("@f", fin);

            cn.Open();

            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Ingrediente
                {
                    IdIngrediente = dr.GetInt32("id_ingrediente"),

                    Nombre =
                        dr["nombre"].ToString(),

                    UnidadMedida =
                        dr["unidad_medida"].ToString(),

                    CaloriasPorU =
                        dr.IsDBNull(
                            dr.GetOrdinal("calorias_por_u"))
                        ? 0
                        : dr.GetDecimal("calorias_por_u"),

                    Imagen =
                        dr.IsDBNull(
                            dr.GetOrdinal("imagen")) ? null : (byte[])dr["imagen"]
                });
            }

            return lista;
        }

        public List<Ingrediente> ObtenerPorNombre(
    string criterio)
        {
            var lista = new List<Ingrediente>();

            using var cn = Conexion.GetConexion();

            using var cmd = new MySqlCommand(
                @"SELECT *
          FROM vw_ingredientes
          WHERE nombre
          LIKE @c",
                cn);

            cmd.Parameters.AddWithValue(
                "@c",
                "%" + criterio + "%");

            cn.Open();

            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Ingrediente
                {
                    IdIngrediente =
                        dr.GetInt32("id_ingrediente"),

                    Nombre =
                        dr["nombre"].ToString(),

                    UnidadMedida =
                        dr["unidad_medida"].ToString(),

                    CaloriasPorU =
                        dr.IsDBNull(
                            dr.GetOrdinal("calorias_por_u"))
                        ? 0
                        : dr.GetDecimal("calorias_por_u"),

                    Imagen =
                        dr.IsDBNull(dr.GetOrdinal("imagen")) ? null: (byte[])dr["imagen"]
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
                    @"SELECT unidad_medida,
                     COUNT(*) total
              FROM vw_ingredientes
              GROUP BY unidad_medida",
                    cn);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            while (dr.Read())
            {
                datos.Add(
                    dr["unidad_medida"].ToString(),
                    dr.GetInt32("total"));
            }

            return datos;
        }

        public Ingrediente ObtenerDetalle(
    int id)
        {
            using var cn =
                Conexion.GetConexion();

            using var cmd =
                new MySqlCommand(
                    @"SELECT *
              FROM vw_ingredientes
              WHERE id_ingrediente=@id",
                    cn);

            cmd.Parameters.AddWithValue(
                "@id",
                id);

            cn.Open();

            using var dr =
                cmd.ExecuteReader();

            if (dr.Read())
            {
                return new Ingrediente
                {
                    IdIngrediente =
                        dr.GetInt32("id_ingrediente"),

                    Nombre =
                        dr["nombre"].ToString(),

                    UnidadMedida =
                        dr["unidad_medida"].ToString(),

                    CaloriasPorU =
                        dr.IsDBNull(
                            dr.GetOrdinal("calorias_por_u"))
                        ? 0
                        : dr.GetDecimal("calorias_por_u"),

                    Imagen =
                        dr.IsDBNull(
                            dr.GetOrdinal("imagen"))
                        ? null
                        : (byte[])dr["imagen"]
                };
            }

            return null;
        }

        public void Insertar(Ingrediente i)
        {
            using var cn = Conexion.GetConexion();
            using var cmd = new MySqlCommand(
                "INSERT INTO ingredientes(nombre,unidad_medida," +
                "calorias_por_u,imagen) VALUES(@n,@u,@c,@img)", cn);
            cmd.Parameters.AddWithValue("@n", i.Nombre);
            cmd.Parameters.AddWithValue("@u", i.UnidadMedida);
            cmd.Parameters.AddWithValue("@c", i.CaloriasPorU);
            cmd.Parameters.AddWithValue("@img",
                i.Imagen ?? (object)DBNull.Value);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var cn = Conexion.GetConexion();
            using var cmd = new MySqlCommand(
                "DELETE FROM ingredientes WHERE id_ingrediente=@id", cn);
            cmd.Parameters.AddWithValue("@id", id);
            cn.Open();
            cmd.ExecuteNonQuery();
        }
        public void Actualizar(Ingrediente i)
        {
            using var cn = Conexion.GetConexion();

            using var cmd = new MySqlCommand(
                "UPDATE ingredientes SET " +
                "nombre=@n," +
                "unidad_medida=@u," +
                "calorias_por_u=@c," +
                "imagen=@img " +
                "WHERE id_ingrediente=@id", cn);

            cmd.Parameters.AddWithValue("@n", i.Nombre);
            cmd.Parameters.AddWithValue("@u", i.UnidadMedida);
            cmd.Parameters.AddWithValue("@c", i.CaloriasPorU);

            cmd.Parameters.AddWithValue(
                "@img",
                i.Imagen ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@id", i.IdIngrediente);

            cn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
