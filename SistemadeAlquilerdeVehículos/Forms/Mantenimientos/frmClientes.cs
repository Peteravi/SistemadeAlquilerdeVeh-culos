using System;
using System.Windows.Forms;
using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;

namespace SistemadeAlquilerdeVehículos.Forms.Mantenimientos
{
    public partial class frmClientes : Form
    {
        private readonly ClienteRepository _clienteRepository;
        private bool _modoEdicion = false;

        public frmClientes()
        {
            InitializeComponent();
            _clienteRepository = new ClienteRepository();
            CargarClientes();
            ConfigurarEventos();
        }

        private void ConfigurarEventos()
        {
            dgvClientes.SelectionChanged += (s, e) => ActualizarEstadoBotones();
            this.Load += (s, e) => ActualizarEstadoBotones();
        }

        private void CargarClientes()
        {
            try
            {
                var clientes = _clienteRepository.ObtenerTodos();
                dgvClientes.DataSource = clientes;
                dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvClientes.ClearSelection();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar clientes: {ex.Message}");
            }
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !_modoEdicion)
            {
                var cliente = (Cliente)dgvClientes.Rows[e.RowIndex].DataBoundItem;
                MostrarDatosCliente(cliente);
            }
        }

        private void MostrarDatosCliente(Cliente cliente)
        {
            txtIdCliente.Text = cliente.IdCliente.ToString();
            txtNombre.Text = cliente.Nombre;
            txtEmail.Text = cliente.Email;
            txtTelefono.Text = cliente.Telefono;
            txtDireccion.Text = cliente.Direccion;
            dtpFechaNacimiento.Value = cliente.FechaNacimiento;
            cmbTipoDocumento.Text = cliente.TipoDocumento;
            txtNumDocumento.Text = cliente.NumDocumento;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            _modoEdicion = true;
            LimpiarCampos();
            HabilitarCampos(true);
            ActualizarEstadoBotones();
            txtNombre.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            try
            {
                var cliente = new Cliente
                {
                    Nombre = txtNombre.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim(),
                    FechaNacimiento = dtpFechaNacimiento.Value,
                    TipoDocumento = cmbTipoDocumento.Text,
                    NumDocumento = txtNumDocumento.Text.Trim()
                };

                if (string.IsNullOrEmpty(txtIdCliente.Text))
                {
                    _clienteRepository.Crear(cliente);
                    MostrarExito("Cliente creado exitosamente.");
                }
                else
                {
                    cliente.IdCliente = int.Parse(txtIdCliente.Text);
                    _clienteRepository.Actualizar(cliente);
                    MostrarExito("Cliente actualizado exitosamente.");
                }

                _modoEdicion = false;
                CargarClientes();
                LimpiarCampos();
                HabilitarCampos(false);
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al guardar cliente: {ex.Message}");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _modoEdicion = false;
            LimpiarCampos();
            HabilitarCampos(false);
            ActualizarEstadoBotones();
            dgvClientes.ClearSelection();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MostrarAdvertencia("Por favor seleccione un cliente.");
                return;
            }

            _modoEdicion = true;
            HabilitarCampos(true);
            ActualizarEstadoBotones();
            txtNombre.Focus();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MostrarAdvertencia("Por favor seleccione un cliente.");
                return;
            }

            if (MessageBox.Show("¿Está seguro que desea eliminar este cliente?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var idCliente = (int)dgvClientes.SelectedRows[0].Cells["IdCliente"].Value;
                    _clienteRepository.Eliminar(idCliente);
                    MostrarExito("Cliente eliminado exitosamente.");
                    CargarClientes();
                    LimpiarCampos();
                    ActualizarEstadoBotones();
                }
                catch (Exception ex)
                {
                    MostrarError($"Error al eliminar cliente: {ex.Message}");
                }
            }
        }

        private void LimpiarCampos()
        {
            txtIdCliente.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            dtpFechaNacimiento.Value = DateTime.Now;
            cmbTipoDocumento.SelectedIndex = -1;
            txtNumDocumento.Text = string.Empty;
        }

        private void HabilitarCampos(bool habilitar)
        {
            txtNombre.Enabled = habilitar;
            txtEmail.Enabled = habilitar;
            txtTelefono.Enabled = habilitar;
            txtDireccion.Enabled = habilitar;
            dtpFechaNacimiento.Enabled = habilitar;
            cmbTipoDocumento.Enabled = habilitar;
            txtNumDocumento.Enabled = habilitar;
        }

        private void ActualizarEstadoBotones()
        {
            bool haySeleccion = dgvClientes.SelectedRows.Count > 0;

            btnNuevo.Enabled = !_modoEdicion;
            btnGuardar.Enabled = _modoEdicion;
            btnCancelar.Enabled = _modoEdicion;
            btnEditar.Enabled = !_modoEdicion && haySeleccion;
            btnEliminar.Enabled = !_modoEdicion && haySeleccion;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MostrarAdvertencia("El nombre es obligatorio.");
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MostrarAdvertencia("Ingrese un email válido.");
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MostrarAdvertencia("El teléfono es obligatorio.");
                txtTelefono.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNumDocumento.Text))
            {
                MostrarAdvertencia("El número de documento es obligatorio.");
                txtNumDocumento.Focus();
                return false;
            }

            if (cmbTipoDocumento.SelectedIndex == -1)
            {
                MostrarAdvertencia("Seleccione un tipo de documento.");
                cmbTipoDocumento.Focus();
                return false;
            }

            return true;
        }

        private void MostrarExito(string mensaje)
        {
            MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MostrarAdvertencia(string mensaje)
        {
            MessageBox.Show(mensaje, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}