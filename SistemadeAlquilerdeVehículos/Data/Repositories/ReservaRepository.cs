using Npgsql;
using SistemadeAlquilerdeVehículos.Data.Models;
using System;
using System.Collections.Generic;
using System.Data; 

namespace SistemadeAlquilerdeVehículos.Data.Repositories
{
    public class ReservaRepository : IDisposable
    {
        private readonly string _connectionString;
        private NpgsqlConnection _connection;

        public ReservaRepository(string connectionString = "Server=localhost;Database=alquiler_vehiculos;User Id=postgres;Password=1234;")
        {
            _connectionString = connectionString;
            _connection = new NpgsqlConnection(_connectionString);
        }

        private void OpenConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();
        }

        private void CloseConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
                _connection.Close();
        }

        public void Dispose()
        {
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }

        public List<Reserva> GetReservasPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            var reservas = new List<Reserva>();

            try
            {
                OpenConnection();

                string query = @"SELECT r.id_reserva, r.id_cliente, r.id_vehiculo, r.num_documento, 
                        r.fecha_reserva, r.fecha_inicio, r.fecha_fin, r.estado,
                        c.nombre as nombre_cliente, v.placa as placa_vehiculo
                        FROM reservas r
                        JOIN clientes c ON r.id_cliente = c.id_cliente
                        JOIN vehiculos v ON r.id_vehiculo = v.id_vehiculo
                        WHERE r.fecha_inicio BETWEEN @FechaInicio AND @FechaFin
                        ORDER BY r.fecha_inicio";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reserva = new Reserva
                            {
                                IdReserva = Convert.ToInt32(reader["id_reserva"]),
                                IdCliente = Convert.ToInt32(reader["id_cliente"]),
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                NumDocumento = reader["num_documento"].ToString(),
                                FechaReserva = Convert.ToDateTime(reader["fecha_reserva"]),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = Convert.ToDateTime(reader["fecha_fin"]),
                                Estado = reader["estado"].ToString(),
                                NombreCliente = reader["nombre_cliente"].ToString(),
                                PlacaVehiculo = reader["placa_vehiculo"].ToString()
                            };

                            // Verificación de cálculo de días
                            Console.WriteLine($"Reserva ID: {reserva.IdReserva}, Días: {reserva.DiasReserva}");

                            reservas.Add(reserva);
                        }
                    }
                }
                return reservas;
            }
            finally
            {
                CloseConnection();
            }
        }
        public Reserva GetUltimaReservaPorVehiculo(int idVehiculo)
        {
            try
            {
                OpenConnection();
                string query = @"SELECT * FROM reservas 
                       WHERE id_vehiculo = @IdVehiculo
                       ORDER BY fecha_fin DESC LIMIT 1";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@IdVehiculo", idVehiculo);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Reserva
                            {
                                IdReserva = Convert.ToInt32(reader["id_reserva"]),
                                IdCliente = Convert.ToInt32(reader["id_cliente"]),
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                NumDocumento = reader["num_documento"].ToString(),
                                FechaReserva = Convert.ToDateTime(reader["fecha_reserva"]),
                                FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                                FechaFin = Convert.ToDateTime(reader["fecha_fin"]),
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

        public bool Create(Reserva reserva)
        {
            try
            {
                OpenConnection();

                using (var transaction = _connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Insertar la reserva
                        string query = @"INSERT INTO reservas 
                                      (id_cliente, id_vehiculo, num_documento, 
                                       fecha_reserva, fecha_inicio, fecha_fin, estado)
                                      VALUES 
                                      (@IdCliente, @IdVehiculo, @NumDocumento, 
                                       @FechaReserva, @FechaInicio, @FechaFin, @Estado)
                                      RETURNING id_reserva";

                        using (var cmd = new NpgsqlCommand(query, _connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@IdCliente", reserva.IdCliente);
                            cmd.Parameters.AddWithValue("@IdVehiculo", reserva.IdVehiculo);
                            cmd.Parameters.AddWithValue("@NumDocumento", reserva.NumDocumento ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@FechaReserva", reserva.FechaReserva);
                            cmd.Parameters.AddWithValue("@FechaInicio", reserva.FechaInicio);
                            cmd.Parameters.AddWithValue("@FechaFin", reserva.FechaFin);
                            cmd.Parameters.AddWithValue("@Estado", reserva.Estado);

                            cmd.ExecuteScalar();
                        }

                        // 2. Actualizar estado del vehículo
                        string updateQuery = "UPDATE vehiculos SET estado = 'reservado' WHERE id_vehiculo = @IdVehiculo";
                        using (var updateCmd = new NpgsqlCommand(updateQuery, _connection, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@IdVehiculo", reserva.IdVehiculo);
                            updateCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error al crear la reserva: " + ex.Message, ex);
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool CancelarReserva(int idReserva)
        {
            try
            {
                OpenConnection();

                using (var transaction = _connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Obtener el vehículo asociado a la reserva
                        int idVehiculo;
                        string getVehiculoQuery = "SELECT id_vehiculo FROM reservas WHERE id_reserva = @IdReserva";

                        using (var cmd = new NpgsqlCommand(getVehiculoQuery, _connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@IdReserva", idReserva);
                            idVehiculo = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 2. Cancelar la reserva
                        string cancelQuery = "UPDATE reservas SET estado = 'cancelada' WHERE id_reserva = @IdReserva";
                        using (var cancelCmd = new NpgsqlCommand(cancelQuery, _connection, transaction))
                        {
                            cancelCmd.Parameters.AddWithValue("@IdReserva", idReserva);
                            cancelCmd.ExecuteNonQuery();
                        }

                        // 3. Actualizar estado del vehículo
                        string updateQuery = "UPDATE vehiculos SET estado = 'disponible' WHERE id_vehiculo = @IdVehiculo";
                        using (var updateCmd = new NpgsqlCommand(updateQuery, _connection, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@IdVehiculo", idVehiculo);
                            updateCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error al cancelar la reserva: " + ex.Message, ex);
                    }
                }
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}