using System;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SistemadeAlquilerdeVehículos.Data.Models
{
    public class Oferta
    {
        public int IdOferta { get; set; }
        public int IdModelo { get; set; }
        public string DescripcionOferta { get; set; }
        public decimal PrecioOferta { get; set; }
        public DateTime LimiteOferta { get; set; }
        public string NombreModelo { get; set; }
    }
}
