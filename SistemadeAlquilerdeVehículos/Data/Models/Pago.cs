using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlquilerdeVehículos.Data.Models
{
    public class Pago
    {
        public int IdPago { get; set; }
        public int IdAlquiler { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal MontoPagado { get; set; }
        public string MetodoPago { get; set; } // tarjeta, efectivo, transferencia
        public virtual Alquiler Alquiler { get; set; }
    }
}