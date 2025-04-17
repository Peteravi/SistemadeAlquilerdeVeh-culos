using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlquilerdeVehículos.Data.Models
{
    public class Alquiler
    {
        public int IdAlquiler { get; set; }
        public int IdCliente { get; set; }
        public int IdVehiculo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal TotalPago { get; set; }
        public string Estado { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Vehiculo Vehiculo { get; set; }
    }
}