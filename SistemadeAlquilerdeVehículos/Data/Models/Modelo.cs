using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlquilerdeVehículos.Data.Models
{
    public class Modelo
    {
        public int IdModelo { get; set; }
        public string NombreModelo { get; set; }
        public char Estado { get; set; }
        public decimal Precio { get; set; } 
    }
}