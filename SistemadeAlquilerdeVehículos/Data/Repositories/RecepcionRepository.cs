using Npgsql;
using SistemadeAlquilerdeVehículos.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SistemadeAlquilerdeVehículos.Data.Repositories
{
    public class RecepcionRepository : IDisposable
    {
        private readonly string _connectionString;
        private NpgsqlConnection _connection;

        public RecepcionRepository(string connectionString = "Server=localhost;Database=alquiler_vehiculos;User Id=postgres;Password=1234;")
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

        public int Create(RecepcionVehiculo recepcion)
        {
            try
            {
                OpenConnection();

                using (var transaction = _connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"INSERT INTO recepcion_vehiculos (id_vehiculo, fecha_recepcion, observacion) 
                                      VALUES (@IdVehiculo, @FechaRecepcion, @Observacion)
                                      RETURNING id_recepcion";

                        int idRecepcion;

                        using (var cmd = new NpgsqlCommand(query, _connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@IdVehiculo", recepcion.IdVehiculo);
                            cmd.Parameters.AddWithValue("@FechaRecepcion", recepcion.FechaRecepcion);
                            cmd.Parameters.AddWithValue("@Observacion",
                                string.IsNullOrEmpty(recepcion.Observacion) ? (object)DBNull.Value : recepcion.Observacion);

                            idRecepcion = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // Actualizar estado del vehículo a disponible
                        string updateQuery = "UPDATE vehiculos SET estado = 'disponible' WHERE id_vehiculo = @IdVehiculo";
                        using (var updateCmd = new NpgsqlCommand(updateQuery, _connection, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@IdVehiculo", recepcion.IdVehiculo);
                            updateCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return idRecepcion;
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

        public RecepcionVehiculo GetById(int id)
        {
            try
            {
                OpenConnection();
                string query = "SELECT * FROM recepcion_vehiculos WHERE id_recepcion = @Id";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new RecepcionVehiculo
                            {
                                IdRecepcion = Convert.ToInt32(reader["id_recepcion"]),
                                IdVehiculo = Convert.ToInt32(reader["id_vehiculo"]),
                                FechaRecepcion = Convert.ToDateTime(reader["fecha_recepcion"]),
                                Observacion = reader["observacion"] != DBNull.Value ? reader["observacion"].ToString() : null
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

        public DataTable GetByDate(DateTime fecha)
        {
            var dt = new DataTable();
            try
            {
                OpenConnection();
                string query = @"SELECT 
                            rv.id_recepcion as IdRecepcion, 
                            v.placa as Placa, 
                            rv.fecha_recepcion as FechaRecepcion, 
                            rv.observacion as Observacion
                        FROM recepcion_vehiculos rv
                        JOIN vehiculos v ON rv.id_vehiculo = v.id_vehiculo
                        WHERE rv.fecha_recepcion::date = @Fecha
                        ORDER BY rv.fecha_recepcion DESC";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@Fecha", fecha.Date);

                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                return dt;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable GetAllWithVehiculoInfo()
        {
            var dt = new DataTable();
            try
            {
                OpenConnection();
                string query = @"SELECT 
                            rv.id_recepcion as IdRecepcion, 
                            v.placa as Placa, 
                            rv.fecha_recepcion as FechaRecepcion, 
                            rv.observacion as Observacion
                        FROM recepcion_vehiculos rv
                        JOIN vehiculos v ON rv.id_vehiculo = v.id_vehiculo
                        ORDER BY rv.fecha_recepcion DESC";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                return dt;
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