using Npgsql;
using SistemadeAlquilerdeVehículos.Data.Models;
using System;
using System.Collections.Generic;

namespace SistemadeAlquilerdeVehículos.Data.Repositories
{
    public class OfertaRepository
    {
        private readonly string _connectionString;

        public OfertaRepository()
        {
            _connectionString = "Server=localhost;Database=alquiler_vehiculos;User Id=postgres;Password=1234;";
        }

        public List<Oferta> ObtenerTodas()
        {
            var ofertas = new List<Oferta>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT o.id_oferta, o.id_modelo, o.descripcion_oferta, 
                           o.precio_oferta, o.limite_oferta, 
                           m.nombre_modelo
                    FROM ofertas o
                    JOIN modelos m ON o.id_modelo = m.id_modelo
                    ORDER BY o.limite_oferta DESC;";

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ofertas.Add(new Oferta
                        {
                            IdOferta = reader.GetInt32(0),
                            IdModelo = reader.GetInt32(1),
                            DescripcionOferta = reader.GetString(2),
                            PrecioOferta = reader.GetDecimal(3),
                            LimiteOferta = reader.GetDateTime(4),
                            NombreModelo = reader.GetString(5)
                        });
                    }
                }
            }
            return ofertas;
        }

        public Oferta ObtenerPorId(int idOferta)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                const string query = @"
                    SELECT o.id_oferta, o.id_modelo, o.descripcion_oferta, 
                           o.precio_oferta, o.limite_oferta, 
                           m.nombre_modelo
                    FROM ofertas o
                    JOIN modelos m ON o.id_modelo = m.id_modelo
                    WHERE o.id_oferta = @idOferta;";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@idOferta", idOferta);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Oferta
                    {
                        IdOferta = reader.GetInt32(0),
                        IdModelo = reader.GetInt32(1),
                        DescripcionOferta = reader.GetString(2),
                        PrecioOferta = reader.GetDecimal(3),
                        LimiteOferta = reader.GetDateTime(4),
                        NombreModelo = reader.GetString(5)
                    };
                }

                return null;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error al obtener la oferta con ID {idOferta}: " + ex.Message, ex);
            }
        }

        public void Crear(Oferta oferta)
        {
            if (oferta == null)
                throw new ArgumentNullException(nameof(oferta));

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                const string query = @"
                    INSERT INTO ofertas 
                        (id_modelo, descripcion_oferta, precio_oferta, limite_oferta) 
                    VALUES 
                        (@idModelo, @descripcion, @precio, @limite)
                    RETURNING id_oferta;";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@idModelo", oferta.IdModelo);
                command.Parameters.AddWithValue("@descripcion", oferta.DescripcionOferta);
                command.Parameters.AddWithValue("@precio", oferta.PrecioOferta);
                command.Parameters.AddWithValue("@limite", oferta.LimiteOferta);

                oferta.IdOferta = (int)command.ExecuteScalar();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Error al crear la oferta: " + ex.Message, ex);
            }
        }

        public void Actualizar(Oferta oferta)
        {
            if (oferta == null)
                throw new ArgumentNullException(nameof(oferta));

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                const string query = @"
                    UPDATE ofertas
                    SET 
                        id_modelo = @idModelo, 
                        descripcion_oferta = @descripcion, 
                        precio_oferta = @precio, 
                        limite_oferta = @limite
                    WHERE id_oferta = @idOferta;";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@idModelo", oferta.IdModelo);
                command.Parameters.AddWithValue("@descripcion", oferta.DescripcionOferta);
                command.Parameters.AddWithValue("@precio", oferta.PrecioOferta);
                command.Parameters.AddWithValue("@limite", oferta.LimiteOferta);
                command.Parameters.AddWithValue("@idOferta", oferta.IdOferta);

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows == 0)
                {
                    throw new Exception($"No se encontró la oferta con ID {oferta.IdOferta} para actualizar.");
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error al actualizar la oferta con ID {oferta.IdOferta}: " + ex.Message, ex);
            }
        }

        public void Eliminar(int idOferta)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                const string query = "DELETE FROM ofertas WHERE id_oferta = @idOferta;";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@idOferta", idOferta);

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows == 0)
                {
                    throw new Exception($"No se encontró la oferta con ID {idOferta} para eliminar.");
                }
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Error al eliminar la oferta con ID {idOferta}: " + ex.Message, ex);
            }
        }

        public decimal GetPrecioMinimoDisponible()
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT MIN(precio_diario) FROM vehiculos WHERE estado = 'Disponible'", conn))
                {
                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }
        }

        public decimal GetPrecioMaximoDisponible()
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT MAX(precio_diario) FROM vehiculos WHERE estado = 'Disponible'", conn))
                {
                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }
        }

        public List<Oferta> ObtenerOfertasDisponibles()
        {
            var ofertas = new List<Oferta>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                SELECT descripcion_oferta, precio_oferta, limite_oferta 
                FROM ofertas 
                WHERE limite_oferta >= @fechaActual
                ORDER BY limite_oferta ASC";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fechaActual", DateTime.Today);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ofertas.Add(new Oferta
                                {
                                    DescripcionOferta = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                                    PrecioOferta = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1),
                                    LimiteOferta = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2)
                                });
                            }
                        }
                    }

                    // Depuración: Ver cuántas ofertas se encontraron
                    Console.WriteLine($"Ofertas encontradas: {ofertas.Count}");
                }
                catch (Exception ex)
                {
                    // Registrar el error para diagnóstico
                    Console.WriteLine($"Error en ObtenerOfertasDisponibles: {ex.Message}");
                    throw;
                }
            }
            return ofertas;
        } 



    }
}