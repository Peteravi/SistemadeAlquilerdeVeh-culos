using Npgsql;
using System;
using System.Windows.Forms;

public static class DatabaseTester
{
    public static void TestConnection(string connectionString = null)
    {
        // Si no se proporciona connectionString, usa la predeterminada
        connectionString = connectionString ?? "Server=localhost;Port=5432;Database=alquiler_vehiculos;User Id=postgres;Password=1234;";

        try
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Verificar si la tabla usuarios existe
                bool tablaUsuariosExiste = VerificarTablaUsuarios(connection);

                string mensaje = "¡Conexión exitosa a PostgreSQL!\n\n";
                mensaje += $"Servidor: {connection.Host}\n";
                mensaje += $"Base de datos: {connection.Database}\n";
                mensaje += $"Usuario: {connection.UserName}\n";
                mensaje += $"Tabla 'usuarios' existe: {(tablaUsuariosExiste ? "Sí" : "No")}";

                // Obtener versión de PostgreSQL
                var versionCommand = new NpgsqlCommand("SELECT version();", connection);
                var version = versionCommand.ExecuteScalar().ToString();
                mensaje += $"\n\nVersión del servidor:\n{version}";

                MessageBox.Show(mensaje, "Prueba de Conexión Exitosa",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (NpgsqlException npgEx)
        {
            string errorMessage = $"Error específico de PostgreSQL:\n\n{npgEx.Message}";

            if (npgEx.InnerException != null)
            {
                errorMessage += $"\n\nDetalles:\n{npgEx.InnerException.Message}";
            }

            MessageBox.Show(errorMessage, "Error de Conexión a PostgreSQL",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error general de conexión:\n\n{ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static bool VerificarTablaUsuarios(NpgsqlConnection connection)
    {
        try
        {
            var command = new NpgsqlCommand(
                "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'usuarios');",
                connection);
            return (bool)command.ExecuteScalar();
        }
        catch
        {
            return false;
        }
    }
}