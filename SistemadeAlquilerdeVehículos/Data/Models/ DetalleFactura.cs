using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlquilerdeVehículos.Data.Models
{
    public class DetalleFactura
    {
        public Alquiler Alquiler { get; set; }
        public string Descripcion { get; set; }
        public int CantidadDias { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }
}