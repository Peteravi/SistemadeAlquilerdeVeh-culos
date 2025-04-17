using Npgsql;
using SistemadeAlquilerdeVehículos.Data.Models;
using System;
using System.Collections.Generic;

namespace SistemadeAlquilerdeVehículos.Data.Repositories
{
    public class MarcaRepository : IDisposable
    {
        private readonly string _connectionString;
        private NpgsqlConnection _connection;

        public MarcaRepository()
        {
            _connectionString = "Server=localhost;Database=alquiler_vehiculos;User Id=postgres;Password=1234;";
        }

        public List<Marca> GetAll()
        {
            var marcas = new List<Marca>();
            using (_connection = new NpgsqlConnection(_connectionString))
            {
                _connection.Open();
                string query = "SELECT id_marca, nombre_marca, estado FROM marcas";

                using (var cmd = new NpgsqlCommand(query, _connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        marcas.Add(new Marca
                        {
                            IdMarca = reader.GetInt32(0),
                            NombreMarca = reader.GetString(1),
                            Estado = reader.GetString(2)[0]
                        });
                    }
                }
            }
            return marcas;
        }

        public List<Marca> GetAllActive()
        {
            var marcas = new List<Marca>();
            using (_connection = new NpgsqlConnection(_connectionString))
            {
                _connection.Open();
                string query = "SELECT id_marca, nombre_marca, estado FROM marcas WHERE estado = 'A'";

                using (var cmd = new NpgsqlCommand(query, _connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        marcas.Add(new Marca
                        {
                            IdMarca = reader.GetInt32(0),
                            NombreMarca = reader.GetString(1),
                            Estado = reader.GetString(2)[0]
                        });
                    }
                }
            }
            return marcas;
        }

        public Marca GetById(int id)
        {
            using (_connection = new NpgsqlConnection(_connectionString))
            {
                _connection.Open();
                string query = "SELECT id_marca, nombre_marca, estado FROM marcas WHERE id_marca = @id";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Marca
                            {
                                IdMarca = reader.GetInt32(0),
                                NombreMarca = reader.GetString(1),
                                Estado = reader.GetString(2)[0]
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool Create(Marca marca)
        {
            using (_connection = new NpgsqlConnection(_connectionString))
            {
                _connection.Open();
                string query = "INSERT INTO marcas (nombre_marca, estado) VALUES (@nombre, @estado)";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@nombre", marca.NombreMarca);
                    cmd.Parameters.AddWithValue("@estado", marca.Estado.ToString());

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(Marca marca)
        {
            using (_connection = new NpgsqlConnection(_connectionString))
            {
                _connection.Open();
                string query = "UPDATE marcas SET nombre_marca = @nombre, estado = @estado WHERE id_marca = @id";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@nombre", marca.NombreMarca);
                    cmd.Parameters.AddWithValue("@estado", marca.Estado.ToString());
                    cmd.Parameters.AddWithValue("@id", marca.IdMarca);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int id)
        {
            using (_connection = new NpgsqlConnection(_connectionString))
            {
                _connection.Open();
                string query = "DELETE FROM marcas WHERE id_marca = @id";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}