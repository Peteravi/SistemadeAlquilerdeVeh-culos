using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlquilerdeVehículos.Data.Models
{
    public class Reserva
    {
        public int IdReserva { get; set; }
        public int IdCliente { get; set; }
        public int IdVehiculo { get; set; }
        public string NumDocumento { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public int DiasReserva => (FechaFin - FechaInicio).Days;
        public string NombreCliente { get; set; }
        public string PlacaVehiculo { get; set; }
    }
}