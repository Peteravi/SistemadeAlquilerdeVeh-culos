using Npgsql;
using SistemadeAlquilerdeVehículos.Data.Models;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Data.Repositories
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository()
        {
            _connectionString = "Server=localhost;Database=alquiler_vehiculos;User Id=postgres;Password=1234;";
        }

        public Usuario ObtenerPorCredenciales(string login, string password)
        {
            Usuario usuario = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var query = "SELECT login_usuario, nombre_usuario, apellidos_usuario, nivel_acceso " +
                           "FROM usuarios WHERE login_usuario = @Login AND pass_usuario = @Password";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                LoginUsuario = reader["login_usuario"].ToString(),
                                NombreUsuario = reader["nombre_usuario"].ToString(),
                                ApellidosUsuario = reader["apellidos_usuario"].ToString(),
                                NivelAcceso = Convert.ToInt32(reader["nivel_acceso"])
                            };
                        }
                    }
                }
            }

            return usuario;
        }

        public bool RegistrarUsuario(Usuario usuario, string password)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var query = @"INSERT INTO usuarios 
                         (login_usuario, pass_usuario, nombre_usuario, 
                          apellidos_usuario, nivel_acceso, email_usuario)
                         VALUES (@Login, @Password, @Nombre, 
                                 @Apellidos, @NivelAcceso, @Email)";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", usuario.LoginUsuario);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Nombre", usuario.NombreUsuario);
                        command.Parameters.AddWithValue("@Apellidos", usuario.ApellidosUsuario);
                        command.Parameters.AddWithValue("@NivelAcceso", usuario.NivelAcceso);
                        command.Parameters.AddWithValue("@Email",
                            string.IsNullOrEmpty(usuario.EmailUsuario) ? (object)DBNull.Value : usuario.EmailUsuario);

                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // Violación de clave única
            {
                MessageBox.Show("El nombre de usuario ya existe", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar usuario: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public List<Usuario> ObtenerTodos()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "SELECT login_usuario, pass_usuario, nivel_acceso, nombre_usuario, apellidos_usuario, email_usuario " +
                    "FROM usuarios", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario
                            {
                                LoginUsuario = reader.GetString(0),
                                PassUsuario = reader.GetString(1),
                                NivelAcceso = reader.GetInt32(2),
                                NombreUsuario = reader.GetString(3),
                                ApellidosUsuario = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                EmailUsuario = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                            });
                        }
                    }
                }
            }

            return usuarios;
        }

        public void Crear(Usuario usuario)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "INSERT INTO usuarios (login_usuario, pass_usuario, nivel_acceso, nombre_usuario, apellidos_usuario, email_usuario) " +
                    "VALUES (@loginUsuario, @passUsuario, @nivelAcceso, @nombreUsuario, @apellidosUsuario, @emailUsuario)", connection))
                {
                    command.Parameters.AddWithValue("@loginUsuario", usuario.LoginUsuario);
                    command.Parameters.AddWithValue("@passUsuario", usuario.PassUsuario);
                    command.Parameters.AddWithValue("@nivelAcceso", usuario.NivelAcceso);
                    command.Parameters.AddWithValue("@nombreUsuario", usuario.NombreUsuario);
                    command.Parameters.AddWithValue("@apellidosUsuario", (object)usuario.ApellidosUsuario ?? DBNull.Value);
                    command.Parameters.AddWithValue("@emailUsuario", (object)usuario.EmailUsuario ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Usuario usuario)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "UPDATE usuarios SET " +
                    "pass_usuario = @passUsuario, " +
                    "nivel_acceso = @nivelAcceso, " +
                    "nombre_usuario = @nombreUsuario, " +
                    "apellidos_usuario = @apellidosUsuario, " +
                    "email_usuario = @emailUsuario " +
                    "WHERE login_usuario = @loginUsuario", connection))
                {
                    command.Parameters.AddWithValue("@loginUsuario", usuario.LoginUsuario);
                    command.Parameters.AddWithValue("@passUsuario", usuario.PassUsuario);
                    command.Parameters.AddWithValue("@nivelAcceso", usuario.NivelAcceso);
                    command.Parameters.AddWithValue("@nombreUsuario", usuario.NombreUsuario);
                    command.Parameters.AddWithValue("@apellidosUsuario", (object)usuario.ApellidosUsuario ?? DBNull.Value);
                    command.Parameters.AddWithValue("@emailUsuario", (object)usuario.EmailUsuario ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string loginUsuario)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "DELETE FROM usuarios WHERE login_usuario = @loginUsuario", connection))
                {
                    command.Parameters.AddWithValue("@loginUsuario", loginUsuario);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool ValidarCredenciales(string loginUsuario, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "SELECT COUNT(*) FROM usuarios WHERE login_usuario = @loginUsuario AND pass_usuario = @password",
                    connection))
                {
                    command.Parameters.AddWithValue("@loginUsuario", loginUsuario);
                    command.Parameters.AddWithValue("@password", password);

                    var count = Convert.ToInt64(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public Usuario ObtenerPorLogin(string loginUsuario)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "SELECT login_usuario, pass_usuario, nivel_acceso, nombre_usuario, apellidos_usuario, email_usuario " +
                    "FROM usuarios WHERE login_usuario = @loginUsuario", connection))
                {
                    command.Parameters.AddWithValue("@loginUsuario", loginUsuario);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                LoginUsuario = reader.GetString(0),
                                PassUsuario = reader.GetString(1),
                                NivelAcceso = reader.GetInt32(2),
                                NombreUsuario = reader.GetString(3),
                                ApellidosUsuario = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                EmailUsuario = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}