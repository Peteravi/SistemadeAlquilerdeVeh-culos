using Npgsql;
using SistemadeAlquilerdeVehículos.Data.Models;
using System;
using System.Collections.Generic;

namespace SistemadeAlquilerdeVehículos.Data.Repositories
{
    public class ModeloRepository
    {
        private readonly string _connectionString;

        public ModeloRepository()
        {
            _connectionString = "Server=localhost;Database=alquiler_vehiculos;User Id=postgres;Password=1234;";
        }

        public bool Add(Modelo modelo)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO modelos (nombre_modelo, estado) 
                                     VALUES (@nombre_modelo, @estado)";

                    cmd.Parameters.AddWithValue("@nombre_modelo", modelo.NombreModelo);
                    cmd.Parameters.AddWithValue("@estado", modelo.Estado);

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (NpgsqlException ex)
                    {
                        throw new Exception("Error al agregar modelo: " + ex.Message, ex);
                    }
                }
            }
        }

        public bool Update(Modelo modelo)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"UPDATE modelos SET 
                                     nombre_modelo = @nombre_modelo, 
                                     estado = @estado
                                     WHERE id_modelo = @id_modelo";

                    cmd.Parameters.AddWithValue("@id_modelo", modelo.IdModelo);
                    cmd.Parameters.AddWithValue("@nombre_modelo", modelo.NombreModelo);
                    cmd.Parameters.AddWithValue("@estado", modelo.Estado);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM modelos WHERE id_modelo = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public Modelo GetById(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM modelos WHERE id_modelo = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Modelo
                            {
                                IdModelo = Convert.ToInt32(reader["id_modelo"]),
                                NombreModelo = reader["nombre_modelo"].ToString(),
                                Estado = Convert.ToChar(reader["estado"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Modelo> GetAll()
        {
            var modelos = new List<Modelo>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM modelos", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        modelos.Add(new Modelo
                        {
                            IdModelo = Convert.ToInt32(reader["id_modelo"]),
                            NombreModelo = reader["nombre_modelo"].ToString(),
                            Estado = Convert.ToChar(reader["estado"])
                        });
                    }
                }
            }
            return modelos;
        }

        public List<Modelo> GetAllActive()
        {
            var modelos = new List<Modelo>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM modelos WHERE estado = 'A'", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        modelos.Add(new Modelo
                        {
                            IdModelo = Convert.ToInt32(reader["id_modelo"]),
                            NombreModelo = reader["nombre_modelo"].ToString(),
                            Estado = Convert.ToChar(reader["estado"])
                        });
                    }
                }
            }
            return modelos;
        }

        public List<Modelo> GetByMarca(int marcaId)
        {
            var modelos = new List<Modelo>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    // Consulta corregida - eliminamos el JOIN innecesario
                    cmd.CommandText = "SELECT * FROM modelos WHERE estado = 'A'";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            modelos.Add(new Modelo
                            {
                                IdModelo = Convert.ToInt32(reader["id_modelo"]),
                                NombreModelo = reader["nombre_modelo"].ToString(),
                                Estado = Convert.ToChar(reader["estado"])
                            });
                        }
                    }
                }
            }
            return modelos;
        }


        public bool Exists(string nombreModelo, int? excludeId = null)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT COUNT(*) FROM modelos 
                                      WHERE nombre_modelo = @nombre_modelo
                                      AND (@excludeId IS NULL OR id_modelo != @excludeId)";
                    cmd.Parameters.AddWithValue("@nombre_modelo", nombreModelo);
                    cmd.Parameters.AddWithValue("@excludeId", excludeId ?? (object)DBNull.Value);

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public decimal ObtenerPrecioNormal(int idModelo)
        {
            decimal precioNormal = 0;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT precio FROM modelos WHERE id_modelo = @idModelo";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idModelo", idModelo);
                    precioNormal = (decimal)command.ExecuteScalar();
                }
            }

            return precioNormal;
        }

        public List<Modelo> ObtenerTodos()
        {
            var modelos = new List<Modelo>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id_modelo, nombre_modelo, estado FROM modelos WHERE estado = 'A'";

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        modelos.Add(new Modelo
                        {
                            IdModelo = reader.GetInt32(0),
                            NombreModelo = reader.GetString(1),
                            Estado = reader.GetChar(2)
                        });
                    }
                }
            }
            return modelos;
        }

        public List<string> GetNombresModelosDisponibles()
        {
            var nombres = new List<string>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT nombre_modelo FROM modelos WHERE estado = 'A' ORDER BY nombre_modelo", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        nombres.Add(reader["nombre_modelo"].ToString());
                    }
                }
            }
            return nombres;
        }

        // Método mejorado para obtener todos los modelos
        public List<Modelo> GetAll(bool soloActivos = false)
        {
            var modelos = new List<Modelo>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                string query = soloActivos ?
                    "SELECT id_modelo, nombre_modelo, estado  FROM modelos WHERE estado = 'A'" :
                    "SELECT id_modelo, nombre_modelo, estado FROM modelos";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        modelos.Add(new Modelo
                        {
                            IdModelo = reader.GetInt32(0),
                            NombreModelo = reader.GetString(1),
                            Estado = reader.GetChar(2),
                            Precio = reader.GetDecimal(3)
                        });
                    }
                }
            }
            return modelos;
        }


    }
}