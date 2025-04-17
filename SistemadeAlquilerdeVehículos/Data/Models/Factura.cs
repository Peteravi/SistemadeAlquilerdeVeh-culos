using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlquilerdeVehículos.Data.Models
{
    public class Factura
    {
        public DateTime Fecha { get; set; }
        public Cliente Cliente { get; set; }
        public List<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();
        public decimal Subtotal => Detalles.Sum(d => d.Total);
        public decimal Impuesto { get; set; }
        public decimal TotalGeneral => Subtotal + Impuesto;
    }
}

