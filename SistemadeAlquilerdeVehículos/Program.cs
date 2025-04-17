using SistemadeAlquilerdeVehículos.Forms;
using System;
using System.Windows.Forms;
using SistemadeAlquilerdeVehículos.Forms.Auth;
using SistemadeAlquilerdeVehículos.Forms.Mantenimientos;
using SistemadeAlquilerdeVehículos.Forms.Consultas;
using SistemadeAlquilerdeVehículos.Forms.Movimientos;
using System.IO;
using System.Drawing;           
static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Mostrar splash screen inicial
        using (var splash = new frmSplash())
        {
            splash.ShowDialog();
        }

        // Mostrar formulario de login
        using (var login = new frmLogin())
        {
            if (login.ShowDialog() == DialogResult.OK)
            {
                // Mostrar splash screen nuevamente antes del formulario principal
                using (var splash = new frmSplash())
                {
                    splash.ShowDialog();
                }

                // Abrir formulario principal
                Application.Run(new frmMain(login.NombreUsuarioCompleto, login.NivelAcceso));
            }
        }
    }
}