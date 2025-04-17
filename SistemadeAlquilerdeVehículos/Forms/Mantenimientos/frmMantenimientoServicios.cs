using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Mantenimientos
{
    public partial class frmMantenimientoServicios : Form
    {
        private readonly MantenimientoRepository _mantenimientoRepo;
        private readonly VehiculoRepository _vehiculoRepo;
        private int? _idMantenimientoActual = null;

        public frmMantenimientoServicios()
        {
            InitializeComponent();
            _mantenimientoRepo = new MantenimientoRepository();
            _vehiculoRepo = new VehiculoRepository();
            ConfigurarEstilos();
        }

        private void ConfigurarEstilos()
        {
            // Estilo para los botones
            btnNuevo.BackColor = Color.FromArgb(21, 101, 192);
            btnGuardar.BackColor = Color.FromArgb(56, 142, 60);
            btnCancelar.BackColor = Color.FromArgb(198, 40, 40);
            btnEliminar.BackColor = Color.FromArgb(211, 47, 47);

            foreach (Control ctrl in panelBotones.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.ForeColor = Color.White;
                    btn.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
                }
            }
        }

        private void frmMantenimientoServicios_Load(object sender, EventArgs e)
        {
            CargarVehiculos();
            CargarMantenimientos();
            LimpiarFormulario();
            HabilitarControles(false);
        }

        private void CargarVehiculos()
        {
            try
            {
                var vehiculos = _vehiculoRepo.GetVehiculosParaCombo();
                cmbVehiculo.DataSource = vehiculos;
                cmbVehiculo.DisplayMember = "InfoCompleta";
                cmbVehiculo.ValueMember = "IdVehiculo";
                cmbVehiculo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar vehículos: {ex.Message}");
            }
        }

        private void dgvMantenimientos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMantenimientos.SelectedRows.Count > 0)
            {
                var selectedRow = dgvMantenimientos.SelectedRows[0];
                _idMantenimientoActual = Convert.ToInt32(selectedRow.Cells["IdMantenimiento"].Value);

                // Buscar el vehículo correspondiente en el ComboBox
                var idVehiculo = Convert.ToInt32(selectedRow.Cells["IdVehiculo"].Value);
                for (int i = 0; i < cmbVehiculo.Items.Count; i++)
                {
                    var vehiculo = cmbVehiculo.Items[i] as VehiculoComboDTO;
                    if (vehiculo?.IdVehiculo == idVehiculo)
                    {
                        cmbVehiculo.SelectedIndex = i;
                        break;
                    }
                }

                txtDescripcion.Text = selectedRow.Cells["Descripcion"].Value.ToString();
                dtpFechaInicio.Value = Convert.ToDateTime(selectedRow.Cells["FechaInicio"].Value);

                var fechaFin = selectedRow.Cells["FechaFin"].Value;
                dtpFechaFin.Value = fechaFin != DBNull.Value ? Convert.ToDateTime(fechaFin) : DateTime.Now;

                txtCosto.Text = Convert.ToDecimal(selectedRow.Cells["Costo"].Value).ToString("N2");

                HabilitarControles(false);
                btnEliminar.Enabled = true;
            }
        }

        private void CargarMantenimientos()
        {
            try
            {
                var mantenimientos = _mantenimientoRepo.GetAllWithVehiculo();
                dgvMantenimientos.DataSource = mantenimientos;

                if (dgvMantenimientos.Columns.Count > 0)
                {
                    // Configurar columnas
                    dgvMantenimientos.Columns["IdMantenimiento"].HeaderText = "ID";
                    dgvMantenimientos.Columns["IdMantenimiento"].Width = 50;
                    dgvMantenimientos.Columns["IdVehiculo"].Visible = false;

                    dgvMantenimientos.Columns["PlacaVehiculo"].HeaderText = "Vehículo";
                    dgvMantenimientos.Columns["PlacaVehiculo"].Width = 120;

                    dgvMantenimientos.Columns["Descripcion"].HeaderText = "Descripción";
                    dgvMantenimientos.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dgvMantenimientos.Columns["FechaInicio"].HeaderText = "Inicio";
                    dgvMantenimientos.Columns["FechaInicio"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvMantenimientos.Columns["FechaInicio"].Width = 90;

                    dgvMantenimientos.Columns["FechaFin"].HeaderText = "Fin";
                    dgvMantenimientos.Columns["FechaFin"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvMantenimientos.Columns["FechaFin"].Width = 90;

                    dgvMantenimientos.Columns["Costo"].HeaderText = "Costo ($)";
                    dgvMantenimientos.Columns["Costo"].DefaultCellStyle.Format = "N2";
                    dgvMantenimientos.Columns["Costo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvMantenimientos.Columns["Costo"].Width = 90;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar mantenimientos: {ex.Message}");
            }
        }

        private void LimpiarFormulario()
        {
            _idMantenimientoActual = null;
            cmbVehiculo.SelectedIndex = -1;
            txtDescripcion.Clear();
            dtpFechaInicio.Value = DateTime.Now;
            dtpFechaFin.Value = DateTime.Now;
            txtCosto.Clear();
            lblPlaca.Text = string.Empty;
        }

        private void HabilitarControles(bool habilitar)
        {
            cmbVehiculo.Enabled = habilitar;
            txtDescripcion.Enabled = habilitar;
            dtpFechaInicio.Enabled = habilitar;
            dtpFechaFin.Enabled = habilitar;
            txtCosto.Enabled = habilitar;

            btnGuardar.Enabled = habilitar;
            btnCancelar.Enabled = habilitar;

            btnNuevo.Enabled = !habilitar;
            btnEliminar.Enabled = !habilitar && dgvMantenimientos.SelectedRows.Count > 0;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            HabilitarControles(true);
            txtDescripcion.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            try
            {
                var mantenimiento = new Mantenimiento
                {
                    IdMantenimiento = _idMantenimientoActual ?? 0,
                    IdVehiculo = (int)cmbVehiculo.SelectedValue,
                    Descripcion = txtDescripcion.Text,
                    FechaInicio = dtpFechaInicio.Value,
                    FechaFin = dtpFechaFin.Value,
                    Costo = decimal.Parse(txtCosto.Text)
                };

                bool resultado;
                string mensaje;

                if (_idMantenimientoActual.HasValue)
                {
                    resultado = _mantenimientoRepo.Update(mantenimiento);
                    mensaje = resultado ? "Mantenimiento actualizado correctamente" : "Error al actualizar el mantenimiento";
                }
                else
                {
                    resultado = _mantenimientoRepo.Add(mantenimiento);
                    mensaje = resultado ? "Mantenimiento registrado correctamente" : "Error al registrar el mantenimiento";
                }

                if (resultado)
                {
                    MostrarMensaje(mensaje, "Éxito", MessageBoxIcon.Information);
                    CargarMantenimientos();
                    LimpiarFormulario();
                    HabilitarControles(false);
                }
                else
                {
                    MostrarMensaje(mensaje, "Error", MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al guardar: {ex.Message}");
            }
        }

        private bool ValidarDatos()
        {
            // Validar vehículo seleccionado
            if (cmbVehiculo.SelectedIndex == -1)
            {
                MostrarAdvertencia("Debe seleccionar un vehículo");
                cmbVehiculo.Focus();
                return false;
            }

            // Validar descripción
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MostrarAdvertencia("La descripción es requerida");
                txtDescripcion.Focus();
                return false;
            }

            // Validar fechas
            if (dtpFechaFin.Value < dtpFechaInicio.Value)
            {
                MostrarAdvertencia("La fecha de fin no puede ser anterior a la fecha de inicio");
                dtpFechaFin.Focus();
                return false;
            }

            // Validar costo
            if (!decimal.TryParse(txtCosto.Text, out decimal costo) || costo <= 0)
            {
                MostrarAdvertencia("El costo debe ser un valor numérico mayor que cero");
                txtCosto.Focus();
                return false;
            }

            return true;
        }

        private void txtCosto_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números, punto decimal y tecla de retroceso
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // Permitir solo un punto decimal
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            HabilitarControles(false);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvMantenimientos.SelectedRows.Count == 0)
            {
                MostrarAdvertencia("Debe seleccionar un mantenimiento para eliminar");
                return;
            }

            var id = Convert.ToInt32(dgvMantenimientos.SelectedRows[0].Cells["IdMantenimiento"].Value);

            if (MessageBox.Show("¿Está seguro que desea eliminar este mantenimiento?", "Confirmación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    bool resultado = _mantenimientoRepo.Delete(id);
                    if (resultado)
                    {
                        MostrarMensaje("Mantenimiento eliminado correctamente", "Éxito", MessageBoxIcon.Information);
                        CargarMantenimientos();
                        LimpiarFormulario();
                    }
                    else
                    {
                        MostrarMensaje("Error al eliminar el mantenimiento", "Error", MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MostrarError($"Error al eliminar: {ex.Message}");
                }
            }
        }

        private void cmbVehiculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVehiculo.SelectedItem is VehiculoComboDTO vehiculo)
            {
                lblPlaca.Text = vehiculo.Placa;
            }
            else
            {
                lblPlaca.Text = string.Empty;
            }
        }

        #region Métodos auxiliares para mensajes
        private void MostrarMensaje(string mensaje, string titulo, MessageBoxIcon icono)
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, icono);
        }

        private void MostrarError(string mensaje)
        {
            MostrarMensaje(mensaje, "Error", MessageBoxIcon.Error);
        }

        private void MostrarAdvertencia(string mensaje)
        {
            MostrarMensaje(mensaje, "Validación", MessageBoxIcon.Warning);
        }
        #endregion
    }
}