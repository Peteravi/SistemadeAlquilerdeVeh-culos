using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Consultas
{
    public partial class frmConsultaMarcas : Form
    {
        private readonly MarcaRepository _marcaRepository;

        public frmConsultaMarcas()
        {
            InitializeComponent();
            _marcaRepository = new MarcaRepository();
        }

        private void frmConsultaMarcas_Load(object sender, EventArgs e)
        {
            CargarMarcas();
        }

        private void CargarMarcas()
        {
            try
            {
                var marcas = _marcaRepository.GetAllActive();
                dgvMarcas.DataSource = marcas;
                ConfigurarDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las marcas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvMarcas.AutoGenerateColumns = false;
            dgvMarcas.Columns.Clear();

            // Configurar columnas del DataGridView
            dgvMarcas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdMarca",
                HeaderText = "ID",
                Name = "colId",
                Width = 50
            });

            dgvMarcas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreMarca",
                HeaderText = "Nombre de la Marca",
                Name = "colNombre",
                Width = 200
            });

            dgvMarcas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Estado",
                HeaderText = "Estado",
                Name = "colEstado",
                Width = 80
            });
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}