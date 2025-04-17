using Npgsql;
using SistemadeAlquilerdeVehículos.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SistemadeAlquilerdeVehículos.Data.Repositories
{
    public class MantenimientoRepository
    {
        private readonly string _connectionString;

        public MantenimientoRepository()
        {
            _connectionString = "Server=localhost;Database=alquiler_vehiculos;User Id=postgres;Password=1234;";
        }

        public bool Add(Mantenimiento mantenimiento)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO mantenimiento 
                                    (id_vehiculo, descripcion, fecha_inicio, fecha_fin, costo) 
                                    VALUES 
                                    (@id_vehiculo, @descripcion, @fecha_inicio, @fecha_fin, @costo)";

                    cmd.Parameters.AddWithValue("@id_vehiculo", mantenimiento.IdVehiculo);
                    cmd.Parameters.AddWithValue("@descripcion", mantenimiento.Descripcion);
                    cmd.Parameters.AddWithValue("@fecha_inicio", mantenimiento.FechaInicio);
                    cmd.Parameters.AddWithValue("@fecha_fin", mantenimiento.FechaFin ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@costo", mantenimiento.Costo);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(Mantenimiento mantenimiento)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"UPDATE mantenimiento SET 
                                    id_vehiculo = @id_vehiculo, 
                                    descripcion = @descripcion, 
                                    fecha_inicio = @fecha_inicio, 
                                    fecha_fin = @fecha_fin, 
                                    costo = @costo
                                    WHERE id_mantenimiento = @id_mantenimiento";

                    cmd.Parameters.AddWithValue("@id_mantenimiento", mantenimiento.IdMantenimiento);
                    cmd.Parameters.AddWithValue("@id_vehiculo", mantenimiento.IdVehiculo);
                    cmd.Parameters.AddWithValue("@descripcion", mantenimiento.Descripcion);
                    cmd.Parameters.AddWithValue("@fecha_inicio", mantenimiento.FechaInicio);
                    cmd.Parameters.AddWithValue("@fecha_fin", mantenimiento.FechaFin ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@costo", mantenimiento.Costo);

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
                    cmd.CommandText = "DELETE FROM mantenimiento WHERE id_mantenimiento = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public Mantenimiento GetById(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM mantenimiento WHERE id_mantenimiento = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Mantenimiento
                            {
                                IdMantenimiento = Convert.ToInt32(reader["id_mantenimiento"]),
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                Descripcion = reader["descripcion"].ToString(),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = reader["fecha_fin"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_fin"]) : (DateTime?)null,
                                Costo = Convert.ToDecimal(reader["costo"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Mantenimiento> GetAll()
        {
            var mantenimientos = new List<Mantenimiento>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM mantenimiento ORDER BY fecha_inicio DESC", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mantenimientos.Add(new Mantenimiento
                        {
                            IdMantenimiento = Convert.ToInt32(reader["id_mantenimiento"]),
                            IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                            Descripcion = reader["descripcion"].ToString(),
                            FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                            FechaFin = reader["fecha_fin"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_fin"]) : (DateTime?)null,
                            Costo = Convert.ToDecimal(reader["costo"])
                        });
                    }
                }
            }
            return mantenimientos;
        }

        public List<MantenimientoDTO> GetAllWithVehiculo()
        {
            var mantenimientos = new List<MantenimientoDTO>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT m.*, v.placa 
                              FROM mantenimiento m
                              JOIN vehiculos v ON m.id_vehiculo = v.id_vehiculo
                              ORDER BY m.fecha_inicio DESC";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mantenimientos.Add(new MantenimientoDTO
                            {
                                IdMantenimiento = Convert.ToInt32(reader["id_mantenimiento"]),
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                Descripcion = reader["descripcion"].ToString(),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = reader["fecha_fin"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_fin"]) : (DateTime?)null,
                                Costo = Convert.ToDecimal(reader["costo"]),
                                PlacaVehiculo = reader["placa"].ToString()
                            });
                        }
                    }
                }
            }
            return mantenimientos;
        }

        public List<Mantenimiento> GetByVehiculo(int idVehiculo)
        {
            var mantenimientos = new List<Mantenimiento>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM mantenimiento WHERE id_vehiculo = @idVehiculo ORDER BY fecha_inicio DESC";
                    cmd.Parameters.AddWithValue("@idVehiculo", idVehiculo);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mantenimientos.Add(new Mantenimiento
                            {
                                IdMantenimiento = Convert.ToInt32(reader["id_mantenimiento"]),
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                Descripcion = reader["descripcion"].ToString(),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = reader["fecha_fin"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_fin"]) : (DateTime?)null,
                                Costo = Convert.ToDecimal(reader["costo"])
                            });
                        }
                    }
                }
            }
            return mantenimientos;
        }

        public List<Mantenimiento> GetByDateRange(DateTime fechaInicio, DateTime fechaFin)
        {
            var mantenimientos = new List<Mantenimiento>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT * FROM mantenimiento 
                                    WHERE fecha_inicio BETWEEN @fechaInicio AND @fechaFin
                                    ORDER BY fecha_inicio DESC";
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFin);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mantenimientos.Add(new Mantenimiento
                            {
                                IdMantenimiento = Convert.ToInt32(reader["id_mantenimiento"]),
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                Descripcion = reader["descripcion"].ToString(),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = reader["fecha_fin"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_fin"]) : (DateTime?)null,
                                Costo = Convert.ToDecimal(reader["costo"])
                            });
                        }
                    }
                }
            }
            return mantenimientos;
        }
    }
}