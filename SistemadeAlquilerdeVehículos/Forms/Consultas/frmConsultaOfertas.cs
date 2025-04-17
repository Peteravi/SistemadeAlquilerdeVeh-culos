using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Consultas
{
    public partial class frmConsultaOfertas : Form
    {
        private readonly OfertaRepository _ofertaRepository;

        public frmConsultaOfertas()
        {
            InitializeComponent();
            _ofertaRepository = new OfertaRepository();
        }

        private void frmConsultaOfertas_Load(object sender, EventArgs e)
        {
            CargarOfertasDisponibles();
        }

        private void CargarOfertasDisponibles()
        {
            try
            {
                dgvOfertas.DataSource = _ofertaRepository.ObtenerOfertasDisponibles();
                ConfigurarDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las ofertas: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvOfertas.AutoGenerateColumns = false;
            dgvOfertas.Columns.Clear();

            dgvOfertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DescripcionOferta",
                HeaderText = "Descripción",
                Width = 300,
                Name = "colDescripcion"
            });

            dgvOfertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioOferta",
                HeaderText = "Precio",
                Width = 100,
                Name = "colPrecio",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvOfertas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LimiteOferta",
                HeaderText = "Válido hasta",
                Width = 120,
                Name = "colLimite",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "d" }
            });
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarOfertasDisponibles();
        }
    }
}