using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Consultas
{
    public partial class frmConsultaClientes : Form
    {
        private readonly ClienteRepository _clienteRepository;

        public frmConsultaClientes()
        {
            InitializeComponent();
            _clienteRepository = new ClienteRepository();
        }

        private void frmConsultaClientes_Load(object sender, EventArgs e)
        {
            CargarClientes();
        }

        private void CargarClientes()
        {
            try
            {
                var clientes = _clienteRepository.ObtenerTodos();
                dgvClientes.DataSource = clientes;
                ConfigurarDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClientes.Columns["IdCliente"].HeaderText = "ID Cliente";
            dgvClientes.Columns["Nombre"].HeaderText = "Nombre Completo";
            dgvClientes.Columns["Email"].HeaderText = "Correo Electrónico";
            dgvClientes.Columns["Telefono"].HeaderText = "Teléfono";
            dgvClientes.Columns["TipoDocumento"].HeaderText = "Tipo Documento";
            dgvClientes.Columns["NumDocumento"].HeaderText = "Número Documento";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim();
            if (string.IsNullOrEmpty(filtro))
            {
                CargarClientes();
                return;
            }

            try
            {
                var clientes = _clienteRepository.ObtenerTodos();
                var clientesFiltrados = clientes.Where(c =>
                    c.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                    c.NumDocumento.Contains(filtro)).ToList();

                dgvClientes.DataSource = clientesFiltrados;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            CargarClientes();
        }
    }
}