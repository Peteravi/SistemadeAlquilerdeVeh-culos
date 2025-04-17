using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeAlquilerdeVehículos.Data.Models
{
    public class Usuario
    {
        public string LoginUsuario { get; set; }
        public string PassUsuario { get; set; }
        public int NivelAcceso { get; set; } // 0=Admin, 1=Usuario normal
        public string NombreUsuario { get; set; }
        public string ApellidosUsuario { get; set; }
        public string EmailUsuario { get; set; }

        public bool EsAdministrador => NivelAcceso == 0;
    }
}
