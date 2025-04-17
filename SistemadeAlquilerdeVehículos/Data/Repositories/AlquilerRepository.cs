using Npgsql;
using SistemadeAlquilerdeVehículos.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SistemadeAlquilerdeVehículos.Data.Repositories
{
    public class AlquilerRepository : IDisposable
    {
        private readonly string _connectionString;
        private NpgsqlConnection _connection;

        public AlquilerRepository(string connectionString = "Server=localhost;Database=alquiler_vehiculos;User Id=postgres;Password=1234;")
        {
            _connectionString = connectionString;
            _connection = new NpgsqlConnection(_connectionString);
        }

        private void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        private void CloseConnection()
        {
            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
        }

        public int Create(Alquiler alquiler)
        {
            try
            {
                OpenConnection();

                // Validar que el vehículo esté disponible
                string validarVehiculo = "SELECT estado FROM vehiculos WHERE id_vehiculo = @IdVehiculo";
                string estadoVehiculo;

                using (var cmdValidar = new NpgsqlCommand(validarVehiculo, _connection))
                {
                    cmdValidar.Parameters.AddWithValue("@IdVehiculo", alquiler.IdVehiculo);
                    estadoVehiculo = cmdValidar.ExecuteScalar()?.ToString();

                    if (estadoVehiculo?.ToLower() != "disponible")
                    {
                        throw new Exception($"El vehículo no está disponible. Estado actual: {estadoVehiculo}");
                    }
                }

                // Validar fechas
                if (alquiler.FechaInicio >= alquiler.FechaFin)
                {
                    throw new Exception("La fecha de fin debe ser posterior a la fecha de inicio");
                }

                if (alquiler.FechaInicio.Date < DateTime.Today)
                {
                    throw new Exception("No se pueden crear alquileres con fecha de inicio en el pasado");
                }

                // Validar que no existan reservas o alquileres en conflicto
                string validarDisponibilidad = @"
                    SELECT COUNT(*) FROM alquileres 
                    WHERE id_vehiculo = @IdVehiculo 
                    AND estado = 'activo'
                    AND (
                        (fecha_inicio <= @FechaFin AND fecha_fin >= @FechaInicio)
                    )
                    
                    UNION ALL
                    
                    SELECT COUNT(*) FROM reservas 
                    WHERE id_vehiculo = @IdVehiculo 
                    AND estado = 'confirmada'
                    AND (
                        (fecha_inicio <= @FechaFin AND fecha_fin >= @FechaInicio)
                    )";

                using (var cmdDisponibilidad = new NpgsqlCommand(validarDisponibilidad, _connection))
                {
                    cmdDisponibilidad.Parameters.AddWithValue("@IdVehiculo", alquiler.IdVehiculo);
                    cmdDisponibilidad.Parameters.AddWithValue("@FechaInicio", alquiler.FechaInicio);
                    cmdDisponibilidad.Parameters.AddWithValue("@FechaFin", alquiler.FechaFin);

                    using (var reader = cmdDisponibilidad.ExecuteReader())
                    {
                        int conflictosAlquiler = 0;
                        int conflictosReserva = 0;

                        if (reader.Read()) conflictosAlquiler = reader.GetInt32(0);
                        if (reader.NextResult() && reader.Read()) conflictosReserva = reader.GetInt32(0);

                        if (conflictosAlquiler > 0 || conflictosReserva > 0)
                        {
                            throw new Exception("El vehículo no está disponible para las fechas seleccionadas");
                        }
                    }
                }

                // Insertar el alquiler
                string query = @"
                    INSERT INTO alquileres 
                    (id_cliente, id_vehiculo, fecha_inicio, fecha_fin, total_pago, estado) 
                    VALUES (@IdCliente, @IdVehiculo, @FechaInicio, @FechaFin, @TotalPago, 'activo')
                    RETURNING id_alquiler";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@IdCliente", alquiler.IdCliente);
                    cmd.Parameters.AddWithValue("@IdVehiculo", alquiler.IdVehiculo);
                    cmd.Parameters.AddWithValue("@FechaInicio", alquiler.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", alquiler.FechaFin);
                    cmd.Parameters.AddWithValue("@TotalPago", alquiler.TotalPago);

                    int idAlquiler = Convert.ToInt32(cmd.ExecuteScalar());

                    // Actualizar estado del vehículo
                    string updateVehiculo = "UPDATE vehiculos SET estado = 'alquilado' WHERE id_vehiculo = @IdVehiculo";
                    using (var updateCmd = new NpgsqlCommand(updateVehiculo, _connection))
                    {
                        updateCmd.Parameters.AddWithValue("@IdVehiculo", alquiler.IdVehiculo);
                        updateCmd.ExecuteNonQuery();
                    }

                    return idAlquiler;
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public Alquiler GetById(int id)
        {
            try
            {
                OpenConnection();
                string query = @"
                    SELECT a.*, c.nombre as cliente_nombre, v.placa, 
                           m.nombre_marca, mod.nombre_modelo
                    FROM alquileres a
                    JOIN clientes c ON a.id_cliente = c.id_cliente
                    JOIN vehiculos v ON a.id_vehiculo = v.id_vehiculo
                    JOIN marcas m ON v.id_marca = m.id_marca
                    JOIN modelos mod ON v.id_modelo = mod.id_modelo
                    WHERE a.id_alquiler = @Id";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Alquiler
                            {
                                IdAlquiler = Convert.ToInt32(reader["id_alquiler"]),
                                IdCliente = Convert.ToInt32(reader["id_cliente"]),
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = Convert.ToDateTime(reader["fecha_fin"]),
                                TotalPago = Convert.ToDecimal(reader["total_pago"]),
                                Estado = reader["estado"].ToString(),
                                Cliente = new Cliente
                                {
                                    Nombre = reader["cliente_nombre"].ToString()
                                },
                                Vehiculo = new Vehiculo
                                {
                                    Placa = reader["placa"].ToString(),
                                    Marca = new Marca
                                    {
                                        NombreMarca = reader["nombre_marca"].ToString()
                                    },
                                    Modelo = new Modelo
                                    {
                                        NombreModelo = reader["nombre_modelo"].ToString()
                                    }
                                }
                            };
                        }
                        return null;
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public IEnumerable<Alquiler> GetByCliente(int idCliente)
        {
            var alquileres = new List<Alquiler>();
            try
            {
                OpenConnection();
                string query = @"
                    SELECT a.*, v.placa, m.nombre_marca, mod.nombre_modelo
                    FROM alquileres a
                    JOIN vehiculos v ON a.id_vehiculo = v.id_vehiculo
                    JOIN marcas m ON v.id_marca = m.id_marca
                    JOIN modelos mod ON v.id_modelo = mod.id_modelo
                    WHERE a.id_cliente = @IdCliente
                    ORDER BY a.fecha_inicio DESC";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@IdCliente", idCliente);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            alquileres.Add(new Alquiler
                            {
                                IdAlquiler = Convert.ToInt32(reader["id_alquiler"]),
                                IdCliente = idCliente,
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = Convert.ToDateTime(reader["fecha_fin"]),
                                TotalPago = Convert.ToDecimal(reader["total_pago"]),
                                Estado = reader["estado"].ToString(),
                                Vehiculo = new Vehiculo
                                {
                                    Placa = reader["placa"].ToString(),
                                    Marca = new Marca
                                    {
                                        NombreMarca = reader["nombre_marca"].ToString()
                                    },
                                    Modelo = new Modelo
                                    {
                                        NombreModelo = reader["nombre_modelo"].ToString()
                                    }
                                }
                            });
                        }
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
            return alquileres;
        }

        public DataTable GetActivos()
        {
            var dt = new DataTable();
            try
            {
                OpenConnection();
                string query = @"
                    SELECT 
                        a.id_alquiler,
                        c.nombre as cliente,
                        v.placa,
                        m.nombre_marca as marca,
                        mod.nombre_modelo as modelo,
                        a.fecha_inicio,
                        a.fecha_fin,
                        a.total_pago,
                        a.estado
                    FROM alquileres a
                    JOIN clientes c ON a.id_cliente = c.id_cliente
                    JOIN vehiculos v ON a.id_vehiculo = v.id_vehiculo
                    JOIN marcas m ON v.id_marca = m.id_marca
                    JOIN modelos mod ON v.id_modelo = mod.id_modelo
                    WHERE a.estado = 'activo'
                    ORDER BY a.fecha_inicio DESC";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
            return dt;
        }

        public bool FinalizarAlquiler(int idAlquiler, DateTime fechaRealFin)
        {
            try
            {
                OpenConnection();

                using (var transaction = _connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Obtener información del alquiler
                        string getAlquiler = "SELECT id_vehiculo, fecha_fin FROM alquileres WHERE id_alquiler = @IdAlquiler";
                        int idVehiculo = 0;
                        DateTime fechaFinOriginal = DateTime.MinValue;

                        using (var cmdGet = new NpgsqlCommand(getAlquiler, _connection, transaction))
                        {
                            cmdGet.Parameters.AddWithValue("@IdAlquiler", idAlquiler);

                            using (var reader = cmdGet.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    idVehiculo = Convert.ToInt32(reader["id_vehiculo"]);
                                    fechaFinOriginal = Convert.ToDateTime(reader["fecha_fin"]);
                                }
                                else
                                {
                                    throw new Exception("Alquiler no encontrado");
                                }
                            }
                        }

                        // 2. Validar fecha de fin
                        if (fechaRealFin < fechaFinOriginal.AddDays(-1))
                        {
                            throw new Exception("La fecha de fin real no puede ser anterior a la fecha de fin original");
                        }

                        // 3. Actualizar alquiler
                        string updateAlquiler = @"
                            UPDATE alquileres 
                            SET estado = 'finalizado', fecha_fin = @FechaRealFin
                            WHERE id_alquiler = @IdAlquiler";

                        using (var cmdUpdate = new NpgsqlCommand(updateAlquiler, _connection, transaction))
                        {
                            cmdUpdate.Parameters.AddWithValue("@IdAlquiler", idAlquiler);
                            cmdUpdate.Parameters.AddWithValue("@FechaRealFin", fechaRealFin);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        // 4. Actualizar estado del vehículo
                        string updateVehiculo = "UPDATE vehiculos SET estado = 'disponible' WHERE id_vehiculo = @IdVehiculo";
                        using (var cmdVehiculo = new NpgsqlCommand(updateVehiculo, _connection, transaction))
                        {
                            cmdVehiculo.Parameters.AddWithValue("@IdVehiculo", idVehiculo);
                            cmdVehiculo.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public Alquiler GetUltimoAlquilerPorVehiculo(int idVehiculo)
        {
            try
            {
                OpenConnection();
                string query = @"SELECT * FROM alquileres 
                       WHERE id_vehiculo = @IdVehiculo
                       ORDER BY fecha_fin DESC LIMIT 1";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Alquiler
                            {
                                IdAlquiler = Convert.ToInt32(reader["id_alquiler"]),
                                IdCliente = Convert.ToInt32(reader["id_cliente"]),
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = Convert.ToDateTime(reader["fecha_fin"]),
                                TotalPago = Convert.ToDecimal(reader["total_pago"]),
                                Estado = reader["estado"].ToString()
                            };
                        }
                        return null;
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}