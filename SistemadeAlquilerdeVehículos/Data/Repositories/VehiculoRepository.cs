using Npgsql;
using SistemadeAlquilerdeVehículos.Data.Models;
using System;
using System.Collections.Generic;

namespace SistemadeAlquilerdeVehículos.Data.Repositories
{
    public class VehiculoRepository
    {
        private readonly string _connectionString;

        public VehiculoRepository()
        {
            _connectionString = "Server=localhost;Database=alquiler_vehiculos;User Id=postgres;Password=1234;";
        }

        public bool Add(Vehiculo vehiculo)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO vehiculos 
                                    (id_marca, id_modelo, año, chasis, placa, color, tipo_vehiculo, precio_diario, estado, kilometraje) 
                                    VALUES 
                                    (@id_marca, @id_modelo, @año, @chasis, @placa, @color, @tipo_vehiculo, @precio_diario, @estado, @kilometraje)";

                    cmd.Parameters.AddWithValue("@id_marca", vehiculo.IdMarca);
                    cmd.Parameters.AddWithValue("@id_modelo", vehiculo.IdModelo);
                    cmd.Parameters.AddWithValue("@año", vehiculo.Año);
                    cmd.Parameters.AddWithValue("@chasis", vehiculo.Chasis);
                    cmd.Parameters.AddWithValue("@placa", vehiculo.Placa);
                    cmd.Parameters.AddWithValue("@color", vehiculo.Color);
                    cmd.Parameters.AddWithValue("@tipo_vehiculo", vehiculo.TipoVehiculo);
                    cmd.Parameters.AddWithValue("@precio_diario", vehiculo.PrecioDiario);
                    cmd.Parameters.AddWithValue("@estado", vehiculo.Estado);
                    cmd.Parameters.AddWithValue("@kilometraje", vehiculo.Kilometraje);

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (NpgsqlException ex) when (ex.SqlState == "23505")
                    {
                        throw new Exception("Ya existe un vehículo con este chasis o placa", ex);
                    }
                }
            }
        }

        public bool Update(Vehiculo vehiculo)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"UPDATE vehiculos SET 
                                    id_marca = @id_marca, 
                                    id_modelo = @id_modelo, 
                                    año = @año, 
                                    chasis = @chasis, 
                                    placa = @placa, 
                                    color = @color, 
                                    tipo_vehiculo = @tipo_vehiculo, 
                                    precio_diario = @precio_diario, 
                                    estado = @estado, 
                                    kilometraje = @kilometraje
                                    WHERE id_vehiculo = @id_vehiculo";

                    cmd.Parameters.AddWithValue("@id_vehiculo", vehiculo.IdVehiculo);
                    cmd.Parameters.AddWithValue("@id_marca", vehiculo.IdMarca);
                    cmd.Parameters.AddWithValue("@id_modelo", vehiculo.IdModelo);
                    cmd.Parameters.AddWithValue("@año", vehiculo.Año);
                    cmd.Parameters.AddWithValue("@chasis", vehiculo.Chasis);
                    cmd.Parameters.AddWithValue("@placa", vehiculo.Placa);
                    cmd.Parameters.AddWithValue("@color", vehiculo.Color);
                    cmd.Parameters.AddWithValue("@tipo_vehiculo", vehiculo.TipoVehiculo);
                    cmd.Parameters.AddWithValue("@precio_diario", vehiculo.PrecioDiario);
                    cmd.Parameters.AddWithValue("@estado", vehiculo.Estado);
                    cmd.Parameters.AddWithValue("@kilometraje", vehiculo.Kilometraje);

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (NpgsqlException ex) when (ex.SqlState == "23505")
                    {
                        throw new Exception("Ya existe un vehículo con este chasis o placa", ex);
                    }
                }
            }
        }

        public bool Delete(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

               
                if (TieneRelaciones(id))
                {
                    throw new Exception("No se puede eliminar el vehículo porque tiene registros relacionados en otras tablas.");
                }

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM vehiculos WHERE id_vehiculo = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (NpgsqlException ex) when (ex.SqlState == "23503")
                    {
                        throw new Exception("No se puede eliminar el vehículo porque tiene registros relacionados en otras tablas.", ex);
                    }
                }
            }
        }

        public Vehiculo GetById(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM vehiculos WHERE id_vehiculo = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Vehiculo
                            {
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                IdMarca = Convert.ToInt32(reader["id_marca"]),
                                IdModelo = Convert.ToInt32(reader["id_modelo"]),
                                Año = Convert.ToInt32(reader["año"]),
                                Chasis = reader["chasis"].ToString(),
                                Placa = reader["placa"].ToString(),
                                Color = reader["color"].ToString(),
                                TipoVehiculo = reader["tipo_vehiculo"].ToString(),
                                PrecioDiario = Convert.ToDecimal(reader["precio_diario"]),
                                Estado = reader["estado"].ToString(),
                                Kilometraje = Convert.ToInt32(reader["kilometraje"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Vehiculo> GetAll()
        {
            var vehiculos = new List<Vehiculo>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM vehiculos", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vehiculos.Add(new Vehiculo
                        {
                            IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                            IdMarca = Convert.ToInt32(reader["id_marca"]),
                            IdModelo = Convert.ToInt32(reader["id_modelo"]),
                            Año = Convert.ToInt32(reader["año"]),
                            Chasis = reader["chasis"].ToString(),
                            Placa = reader["placa"].ToString(),
                            Color = reader["color"].ToString(),
                            TipoVehiculo = reader["tipo_vehiculo"].ToString(),
                            PrecioDiario = Convert.ToDecimal(reader["precio_diario"]),
                            Estado = reader["estado"].ToString(),
                            Kilometraje = Convert.ToInt32(reader["kilometraje"])
                        });
                    }
                }
            }
            return vehiculos;
        }

        public List<Vehiculo> GetByFilter(string filter, string value)
        {
            var vehiculos = new List<Vehiculo>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    string query = "SELECT * FROM vehiculos WHERE ";
                    switch (filter.ToLower())
                    {
                        case "placa":
                            query += "placa ILIKE @value";
                            break;
                        case "estado":
                            query += "estado ILIKE @value";
                            break;
                        default:
                            query += "placa ILIKE @value OR estado ILIKE @value";
                            break;
                    }

                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@value", $"%{value}%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vehiculos.Add(new Vehiculo
                            {
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                IdMarca = Convert.ToInt32(reader["id_marca"]),
                                IdModelo = Convert.ToInt32(reader["id_modelo"]),
                                Año = Convert.ToInt32(reader["año"]),
                                Chasis = reader["chasis"].ToString(),
                                Placa = reader["placa"].ToString(),
                                Color = reader["color"].ToString(),
                                TipoVehiculo = reader["tipo_vehiculo"].ToString(),
                                PrecioDiario = Convert.ToDecimal(reader["precio_diario"]),
                                Estado = reader["estado"].ToString(),
                                Kilometraje = Convert.ToInt32(reader["kilometraje"])
                            });
                        }
                    }
                }
            }
            return vehiculos;
        }

        public bool ExistsByChasisOrPlaca(string chasis, string placa, int? excludeId = null)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT COUNT(*) FROM vehiculos 
                                      WHERE (chasis = @chasis OR placa = @placa)
                                      AND (@excludeId IS NULL OR id_vehiculo != @excludeId)";
                    cmd.Parameters.AddWithValue("@chasis", chasis);
                    cmd.Parameters.AddWithValue("@placa", placa);
                    cmd.Parameters.AddWithValue("@excludeId", excludeId ?? (object)DBNull.Value);

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public bool TieneAlquileresActivos(int idVehiculo)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT COUNT(*) FROM alquileres 
                                     WHERE id_vehiculo = @idVehiculo 
                                     AND estado = 'Activo'";
                    cmd.Parameters.AddWithValue("@idVehiculo", idVehiculo);

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public bool TieneRelaciones(int idVehiculo)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();

                // Verificar en alquileres
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT COUNT(*) FROM alquileres WHERE id_vehiculo = @id";
                    cmd.Parameters.AddWithValue("@id", idVehiculo);
                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0) return true;
                }

                // Verificar en mantenimientos
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT COUNT(*) FROM mantenimiento WHERE id_vehiculo = @id";
                    cmd.Parameters.AddWithValue("@id", idVehiculo);
                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0) return true;
                }

                // Verificar en reservas
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT COUNT(*) FROM reservas WHERE id_vehiculo = @id";
                    cmd.Parameters.AddWithValue("@id", idVehiculo);
                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0) return true;
                }

                // Verificar en recepción de vehículos
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT COUNT(*) FROM recepcion_vehiculos WHERE id_vehiculo = @id";
                    cmd.Parameters.AddWithValue("@id", idVehiculo);
                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0) return true;
                }

                return false;
            }
        }

        public List<VehiculoDTO> GetAllWithMarcaModelo()
        {
            var vehiculos = new List<VehiculoDTO>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT v.*, m.nombre_marca, mo.nombre_modelo 
                              FROM vehiculos v
                              JOIN marcas m ON v.id_marca = m.id_marca
                              JOIN modelos mo ON v.id_modelo = mo.id_modelo
                              ORDER BY m.nombre_marca, mo.nombre_modelo";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vehiculos.Add(new VehiculoDTO
                            {
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                IdMarca = Convert.ToInt32(reader["id_marca"]),
                                IdModelo = Convert.ToInt32(reader["id_modelo"]),
                                Año = Convert.ToInt32(reader["año"]),
                                Chasis = reader["chasis"].ToString(),
                                Placa = reader["placa"].ToString(),
                                Color = reader["color"].ToString(),
                                TipoVehiculo = reader["tipo_vehiculo"].ToString(),
                                PrecioDiario = Convert.ToDecimal(reader["precio_diario"]),
                                Estado = reader["estado"].ToString(),
                                Kilometraje = Convert.ToInt32(reader["kilometraje"]),
                                NombreMarca = reader["nombre_marca"].ToString(),
                                NombreModelo = reader["nombre_modelo"].ToString()
                            });
                        }
                    }
                }
            }
            return vehiculos;
        }

        public List<VehiculoDTO> GetByPlaca(string placa)
        {
            var vehiculos = new List<VehiculoDTO>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT v.*, m.nombre_marca, mo.nombre_modelo 
                              FROM vehiculos v
                              JOIN marcas m ON v.id_marca = m.id_marca
                              JOIN modelos mo ON v.id_modelo = mo.id_modelo
                              WHERE v.placa ILIKE @placa";
                    cmd.Parameters.AddWithValue("@placa", $"%{placa}%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vehiculos.Add(new VehiculoDTO
                            {
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                IdMarca = Convert.ToInt32(reader["id_marca"]),
                                IdModelo = Convert.ToInt32(reader["id_modelo"]),
                                Año = Convert.ToInt32(reader["año"]),
                                Chasis = reader["chasis"].ToString(),
                                Placa = reader["placa"].ToString(),
                                Color = reader["color"].ToString(),
                                TipoVehiculo = reader["tipo_vehiculo"].ToString(),
                                PrecioDiario = Convert.ToDecimal(reader["precio_diario"]),
                                Estado = reader["estado"].ToString(),
                                Kilometraje = Convert.ToInt32(reader["kilometraje"]),
                                NombreMarca = reader["nombre_marca"].ToString(),
                                NombreModelo = reader["nombre_modelo"].ToString()
                            });
                        }
                    }
                }
            }
            return vehiculos;
        }

        public List<VehiculoDTO> GetDisponibles()
        {
            return GetByEstado("disponible");
        }

        public List<VehiculoDTO> GetRentados()
        {
            return GetByEstado("rentado");
        }

        private List<VehiculoDTO> GetByEstado(string estado)
        {
            var vehiculos = new List<VehiculoDTO>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT v.*, m.nombre_marca, mo.nombre_modelo 
                              FROM vehiculos v
                              JOIN marcas m ON v.id_marca = m.id_marca
                              JOIN modelos mo ON v.id_modelo = mo.id_modelo
                              WHERE v.estado ILIKE @estado";
                    cmd.Parameters.AddWithValue("@estado", estado);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vehiculos.Add(new VehiculoDTO
                            {
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                IdMarca = Convert.ToInt32(reader["id_marca"]),
                                IdModelo = Convert.ToInt32(reader["id_modelo"]),
                                Año = Convert.ToInt32(reader["año"]),
                                Chasis = reader["chasis"].ToString(),
                                Placa = reader["placa"].ToString(),
                                Color = reader["color"].ToString(),
                                TipoVehiculo = reader["tipo_vehiculo"].ToString(),
                                PrecioDiario = Convert.ToDecimal(reader["precio_diario"]),
                                Estado = reader["estado"].ToString(),
                                Kilometraje = Convert.ToInt32(reader["kilometraje"]),
                                NombreMarca = reader["nombre_marca"].ToString(),
                                NombreModelo = reader["nombre_modelo"].ToString()
                            });
                        }
                    }
                }
            }
            return vehiculos;
        }

        public List<VehiculoDTO> GetByMarca(int idMarca)
        {
            var vehiculos = new List<VehiculoDTO>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT v.*, m.nombre_marca, mo.nombre_modelo 
                              FROM vehiculos v
                              JOIN marcas m ON v.id_marca = m.id_marca
                              JOIN modelos mo ON v.id_modelo = mo.id_modelo
                              WHERE v.id_marca = @idMarca";
                    cmd.Parameters.AddWithValue("@idMarca", idMarca);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vehiculos.Add(new VehiculoDTO
                            {
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                IdMarca = Convert.ToInt32(reader["id_marca"]),
                                IdModelo = Convert.ToInt32(reader["id_modelo"]),
                                Año = Convert.ToInt32(reader["año"]),
                                Chasis = reader["chasis"].ToString(),
                                Placa = reader["placa"].ToString(),
                                Color = reader["color"].ToString(),
                                TipoVehiculo = reader["tipo_vehiculo"].ToString(),
                                PrecioDiario = Convert.ToDecimal(reader["precio_diario"]),
                                Estado = reader["estado"].ToString(),
                                Kilometraje = Convert.ToInt32(reader["kilometraje"]),
                                NombreMarca = reader["nombre_marca"].ToString(),
                                NombreModelo = reader["nombre_modelo"].ToString()
                            });
                        }
                    }
                }
            }
            return vehiculos;
        }

        public List<VehiculoDTO> GetByPrecioRange(decimal precioMin, decimal precioMax)
        {
            var vehiculos = new List<VehiculoDTO>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT v.*, m.nombre_marca, mo.nombre_modelo 
                              FROM vehiculos v
                              JOIN marcas m ON v.id_marca = m.id_marca
                              JOIN modelos mo ON v.id_modelo = mo.id_modelo
                              WHERE v.precio_diario BETWEEN @precioMin AND @precioMax
                              ORDER BY v.precio_diario";
                    cmd.Parameters.AddWithValue("@precioMin", precioMin);
                    cmd.Parameters.AddWithValue("@precioMax", precioMax);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vehiculos.Add(new VehiculoDTO
                            {
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                IdMarca = Convert.ToInt32(reader["id_marca"]),
                                IdModelo = Convert.ToInt32(reader["id_modelo"]),
                                Año = Convert.ToInt32(reader["año"]),
                                Chasis = reader["chasis"].ToString(),
                                Placa = reader["placa"].ToString(),
                                Color = reader["color"].ToString(),
                                TipoVehiculo = reader["tipo_vehiculo"].ToString(),
                                PrecioDiario = Convert.ToDecimal(reader["precio_diario"]),
                                Estado = reader["estado"].ToString(),
                                Kilometraje = Convert.ToInt32(reader["kilometraje"]),
                                NombreMarca = reader["nombre_marca"].ToString(),
                                NombreModelo = reader["nombre_modelo"].ToString()
                            });
                        }
                    }
                }
            }
            return vehiculos;
        }

        public List<Vehiculo> GetVehiculosNoDisponibles()
        {
            var vehiculos = new List<Vehiculo>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT * FROM vehiculos 
                             WHERE estado NOT IN ('disponible', 'Disponible')
                             ORDER BY placa";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vehiculos.Add(new Vehiculo
                            {
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                IdMarca = Convert.ToInt32(reader["id_marca"]),
                                IdModelo = Convert.ToInt32(reader["id_modelo"]),
                                Año = Convert.ToInt32(reader["año"]),
                                Chasis = reader["chasis"].ToString(),
                                Placa = reader["placa"].ToString(),
                                Color = reader["color"].ToString(),
                                TipoVehiculo = reader["tipo_vehiculo"].ToString(),
                                PrecioDiario = Convert.ToDecimal(reader["precio_diario"]),
                                Estado = reader["estado"].ToString(),
                                Kilometraje = Convert.ToInt32(reader["kilometraje"])
                            });
                        }
                    }
                }
            }
            return vehiculos;
        }

        public List<VehiculoComboDTO> GetVehiculosParaCombo()
        {
            var vehiculos = new List<VehiculoComboDTO>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT v.id_vehiculo, v.placa, m.nombre_marca, mo.nombre_modelo, v.estado
                      FROM vehiculos v
                      JOIN marcas m ON v.id_marca = m.id_marca
                      JOIN modelos mo ON v.id_modelo = mo.id_modelo
                      ORDER BY v.placa";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vehiculos.Add(new VehiculoComboDTO
                            {
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                Placa = reader["placa"].ToString(),
                                Marca = reader["nombre_marca"].ToString(),
                                Modelo = reader["nombre_modelo"].ToString(),
                                InfoCompleta = $"{reader["placa"]} - {reader["nombre_marca"]} {reader["nombre_modelo"]} (Estado: {reader["estado"]})"
                            });
                        }
                    }
                }
            }
            return vehiculos;
        }

    }
}
         

   