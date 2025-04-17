using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SistemadeAlquilerdeVehículos.Forms.Mantenimientos;
using SistemadeAlquilerdeVehículos.Forms.Consultas;
using SistemadeAlquilerdeVehículos.Forms.Movimientos;


namespace SistemadeAlquilerdeVehículos.Forms.Auth
{
    public partial class frmMain : Form
    {
        // Propiedades para almacenar la información del usuario
        public string UsuarioActual { get; set; }
        public int NivelAcceso { get; set; }

        public frmMain(string nombreUsuario, int nivelAcceso)
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;

            // Asignamos los valores recibidos
            UsuarioActual = nombreUsuario;
            NivelAcceso = nivelAcceso;
        }

        private void ConfigurarAccesoSegunNivel()
        {
            if (NivelAcceso == 1) // Usuario normal
            {
                mantenimientosToolStripMenuItem.Visible = false;
                //movimientosToolStripMenuItem.DropDownItems["registrarNuevoUsuarioToolStripMenuItem"].Visible = false;

                // Deshabilitar opciones específicas
                vehiculosToolStripMenuItem.Enabled = false;
                ofertasToolStripMenuItem.Enabled = false;
                marcasToolStripMenuItem.Enabled = false;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblEstado.Text = "Sistema listo - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            lblUsuario.Text = "Usuario: " + (UsuarioActual ?? "Invitado");
            ConfigurarFondo();
            ConfigurarAccesoSegunNivel();
        }


        private void ConfigurarFondo()
        {
            try
            {
                picFondo.Dock = DockStyle.Fill;
                picFondo.SizeMode = PictureBoxSizeMode.StretchImage;
                picFondo.BackColor = Color.Transparent;

                string rutaImagen = Path.Combine(Application.StartupPath, "Resources", "fondo.png");

                if (File.Exists(rutaImagen))
                {
                    using (var tempImage = Image.FromFile(rutaImagen))
                    {
                        picFondo.Image = new Bitmap(tempImage);
                    }
                }
                else
                {
                    picFondo.BackColor = Color.SteelBlue;
                    MessageBox.Show("No se encontró la imagen de fondo en: " + rutaImagen,
                                  "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar el fondo: {ex.Message}");
                picFondo.BackColor = Color.SteelBlue;
            }
        }

        protected override void OnMdiChildActivate(EventArgs e)
        {
            base.OnMdiChildActivate(e);
            picFondo.SendToBack();
        }

        #region Eventos del Menú

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmClientes>();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmUsuarios>();
        }

        private void vehiculosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmVehiculos>();
        }

        private void marcasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmMarcas>();
        }

        private void modelosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmModelos>();
        }

        private void mantenimientosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //AbrirFormulario<frmMantenimientos>();
        }

        private void mantenimientoServiciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmMantenimientoServicios>();
        }

        private void ofertasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmOfertas>();
        }

        private void facturaClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmFacturaClienteAlquilado>();
        }

        private void recepcionVehiculosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AbrirFormulario<frmRecepcionVehiculos>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir formulario: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Consultas
        private void clientesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaClientes>();
        }

        private void clientesPorIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaClientesPorId>();
        }

        private void clientesPorRangoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaClientesPorRango>();
        }

        private void vehiculosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaVehiculos>();
        }

        private void marcasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaMarcas>();
        }

        private void modelosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaModelos>();
        }

        private void vehiculosPorMatriculaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaVehiculosPorMatricula>();
        }

        private void vehiculosDisponiblesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaVehiculosDisponibles>();
        }

        private void vehiculosRentadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaVehiculosRentados>();
        }

        private void vehiculosPorMarcaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaVehiculosPorMarca>();
        }

        private void ofertasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaOfertas>();
        }

        private void vehiculosPorPrecioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaVehiculosPorPrecio>();
        }

        private void reservasPorFechasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaReservasPorFechas>();
        }

        private void reservasPorDiasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaReservasPorDias>();
        }

        private void recepcionPorFechaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario<frmConsultaRecepcionPorFecha>();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CerrarAplicacion();
        }

        #endregion

        #region Métodos Auxiliares

        private void AbrirFormulario<T>() where T : Form, new()
        {
            try
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form is T)
                    {
                        form.Activate();
                        form.BringToFront();
                        return;
                    }
                }

                T nuevoForm = new T();
                nuevoForm.MdiParent = this;
                nuevoForm.Show();
                nuevoForm.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir formulario: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CerrarAplicacion()
        {
            if (MessageBox.Show("¿Está seguro que desea salir del sistema?", "Confirmar",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        #endregion
    }
}