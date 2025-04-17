using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlquilerdeVehículos.Data.Models
{
    public class Vehiculo
    {
        public int IdVehiculo { get; set; }
        public int IdMarca { get; set; }
        public int IdModelo { get; set; }
        public int Año { get; set; }
        public string Chasis { get; set; }
        public string Placa { get; set; }
        public string Color { get; set; }
        public string TipoVehiculo { get; set; }
        public decimal PrecioDiario { get; set; }
        public string Estado { get; set; }
        public int Kilometraje { get; set; }
        public virtual Marca Marca { get; set; }
        public virtual Modelo Modelo { get; set; }
    }
}