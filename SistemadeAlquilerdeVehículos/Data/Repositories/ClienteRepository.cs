using Npgsql;
using SistemadeAlquilerdeVehículos.Data.Models;
using System;
using System.Collections.Generic;

namespace SistemadeAlquilerdeVehículos.Data.Repositories
{
    public class ClienteRepository
    {
        private readonly string _connectionString;

        public ClienteRepository()
        {
            _connectionString = "Server=localhost;Database=alquiler_vehiculos;User Id=postgres;Password=1234;";
        }

        public List<Cliente> ObtenerTodos()
        {
            var clientes = new List<Cliente>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "SELECT id_cliente, nombre, email, telefono, direccion, " +
                    "fecha_nacimiento, tipo_documento, num_documento " +
                    "FROM clientes ORDER BY nombre", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(new Cliente
                            {
                                IdCliente = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Email = reader.GetString(2),
                                Telefono = reader.GetString(3),
                                Direccion = reader.GetString(4),
                                FechaNacimiento = reader.GetDateTime(5),
                                TipoDocumento = reader.GetString(6),
                                NumDocumento = reader.GetString(7)
                            });
                        }
                    }
                }
            }

            return clientes;
        }

        public Cliente ObtenerPorId(int idCliente)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "SELECT id_cliente, nombre, email, telefono, direccion, " +
                    "fecha_nacimiento, tipo_documento, num_documento " +
                    "FROM clientes WHERE id_cliente = @idCliente", connection))
                {
                    command.Parameters.AddWithValue("@idCliente", idCliente);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cliente
                            {
                                IdCliente = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Email = reader.GetString(2),
                                Telefono = reader.GetString(3),
                                Direccion = reader.GetString(4),
                                FechaNacimiento = reader.GetDateTime(5),
                                TipoDocumento = reader.GetString(6),
                                NumDocumento = reader.GetString(7)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Crear(Cliente cliente)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "INSERT INTO clientes (nombre, email, telefono, direccion, " +
                    "fecha_nacimiento, tipo_documento, num_documento) " +
                    "VALUES (@nombre, @email, @telefono, @direccion, " +
                    "@fechaNacimiento, @tipoDocumento, @numDocumento)", connection))
                {
                    command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@email", cliente.Email);
                    command.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    command.Parameters.AddWithValue("@direccion", cliente.Direccion);
                    command.Parameters.AddWithValue("@fechaNacimiento", cliente.FechaNacimiento);
                    command.Parameters.AddWithValue("@tipoDocumento", cliente.TipoDocumento);
                    command.Parameters.AddWithValue("@numDocumento", cliente.NumDocumento);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Cliente cliente)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "UPDATE clientes SET " +
                    "nombre = @nombre, " +
                    "email = @email, " +
                    "telefono = @telefono, " +
                    "direccion = @direccion, " +
                    "fecha_nacimiento = @fechaNacimiento, " +
                    "tipo_documento = @tipoDocumento, " +
                    "num_documento = @numDocumento " +
                    "WHERE id_cliente = @idCliente", connection))
                {
                    command.Parameters.AddWithValue("@idCliente", cliente.IdCliente);
                    command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@email", cliente.Email);
                    command.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    command.Parameters.AddWithValue("@direccion", cliente.Direccion);
                    command.Parameters.AddWithValue("@fechaNacimiento", cliente.FechaNacimiento);
                    command.Parameters.AddWithValue("@tipoDocumento", cliente.TipoDocumento);
                    command.Parameters.AddWithValue("@numDocumento", cliente.NumDocumento);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int idCliente)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "DELETE FROM clientes WHERE id_cliente = @idCliente", connection))
                {
                    command.Parameters.AddWithValue("@idCliente", idCliente);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool ExisteDocumento(string numDocumento, int? idClienteExcluir = null)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM clientes WHERE num_documento = @numDocumento";

                if (idClienteExcluir.HasValue)
                {
                    query += " AND id_cliente != @idClienteExcluir";
                }

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@numDocumento", numDocumento);

                    if (idClienteExcluir.HasValue)
                    {
                        command.Parameters.AddWithValue("@idClienteExcluir", idClienteExcluir.Value);
                    }

                    var count = Convert.ToInt64(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}