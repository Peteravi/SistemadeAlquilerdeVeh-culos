using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlquilerdeVehículos.Data.Models
{
    public class VehiculoDTO : Vehiculo
    {
        public string NombreMarca { get; set; }
        public string NombreModelo { get; set; }
    }
}