using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Consultas
{
    public partial class frmConsultaModelos : Form
    {
        private readonly ModeloRepository _modeloRepository;

        public frmConsultaModelos()
        {
            InitializeComponent();
            _modeloRepository = new ModeloRepository();
        }

        private void frmConsultaModelos_Load(object sender, EventArgs e)
        {
            CargarModelosDisponibles();
        }

        private void CargarModelosDisponibles()
        {
            try
            {
                lstModelos.Items.Clear();
                var modelos = _modeloRepository.GetNombresModelosDisponibles();

                foreach (var modelo in modelos)
                {
                    lstModelos.Items.Add(modelo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los modelos: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarModelosDisponibles();
        }
    }
}