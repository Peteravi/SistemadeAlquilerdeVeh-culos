using System;
using SistemadeAlquilerdeVehículos.Data.Models;
namespace SistemadeAlquilerdeVehículos
{
    public static class Globales
    {
        public static Usuario UsuarioActual { get; set; }

        
        public static string ConnectionString { get; set; } = "Server=localhost;Database=alquiler_vehiculos;User Id=postgres;Password=1234;";
    }
}
