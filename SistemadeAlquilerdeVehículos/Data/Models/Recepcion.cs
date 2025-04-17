using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlquilerdeVehículos.Data.Models
{
    public class RecepcionVehiculo
    {
        public int IdRecepcion { get; set; }
        public int IdVehiculo { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public string Observacion { get; set; }
        public virtual Vehiculo Vehiculo { get; set; }
    }
}