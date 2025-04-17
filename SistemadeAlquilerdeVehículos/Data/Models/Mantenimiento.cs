using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlquilerdeVehículos.Data.Models
{
    public class Mantenimiento
    {
        public int IdMantenimiento { get; set; }
        public int IdVehiculo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public decimal Costo { get; set; }
        public virtual Vehiculo Vehiculo { get; set; }
    }
}