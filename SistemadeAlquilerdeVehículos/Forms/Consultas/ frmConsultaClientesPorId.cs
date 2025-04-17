using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Consultas
{
    public partial class frmConsultaClientesPorId : Form
    {
        private readonly ClienteRepository _clienteRepository;

        public frmConsultaClientesPorId()
        {
            InitializeComponent();
            _clienteRepository = new ClienteRepository();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIdCliente.Text, out int idCliente))
            {
                MessageBox.Show("Por favor ingrese un ID válido", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var cliente = _clienteRepository.ObtenerPorId(idCliente);
                if (cliente == null)
                {
                    MessageBox.Show("No se encontró un cliente con ese ID", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MostrarCliente(cliente);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarCliente(Cliente cliente)
        {
            txtNombre.Text = cliente.Nombre;
            txtEmail.Text = cliente.Email;
            txtTelefono.Text = cliente.Telefono;
            txtDireccion.Text = cliente.Direccion;
            dtpFechaNacimiento.Value = cliente.FechaNacimiento;
            txtTipoDocumento.Text = cliente.TipoDocumento;
            txtNumDocumento.Text = cliente.NumDocumento;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtIdCliente.Clear();
            txtNombre.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            dtpFechaNacimiento.Value = DateTime.Now;
            txtTipoDocumento.Clear();
            txtNumDocumento.Clear();
        }
    }
}