using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Consultas
{
    public partial class frmConsultaClientesPorRango :Form
    {
        private readonly ClienteRepository _clienteRepository;

        public frmConsultaClientesPorRango()
        {
            InitializeComponent();
            _clienteRepository = new ClienteRepository();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIdInicio.Text, out int idInicio))
            {
                MessageBox.Show("Por favor ingrese un ID inicial válido", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtIdFin.Text, out int idFin))
            {
                MessageBox.Show("Por favor ingrese un ID final válido", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (idInicio > idFin)
            {
                MessageBox.Show("El ID inicial no puede ser mayor que el ID final", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var todosClientes = _clienteRepository.ObtenerTodos();
                var clientesEnRango = todosClientes.Where(c => c.IdCliente >= idInicio && c.IdCliente <= idFin).ToList();

                if (clientesEnRango.Count == 0)
                {
                    MessageBox.Show("No se encontraron clientes en el rango especificado", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dgvClientes.DataSource = clientesEnRango;
                ConfigurarDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtIdInicio.Clear();
            txtIdFin.Clear();
            dgvClientes.DataSource = null;
        }
    }
}