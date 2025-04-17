using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Mantenimientos
{
    public partial class frmVehiculos : Form
    {
        private readonly VehiculoRepository _vehiculoRepository;
        private readonly MarcaRepository _marcaRepository;
        private readonly ModeloRepository _modeloRepository;
        private int _currentId = 0;

        public frmVehiculos()
        {
            InitializeComponent();
            _vehiculoRepository = new VehiculoRepository();
            _marcaRepository = new MarcaRepository();
            _modeloRepository = new ModeloRepository();
        }

        private void frmVehiculos_Load(object sender, EventArgs e)
        {
            CargarMarcas();
            CargarVehiculos();
            LimpiarCampos();
            ConfigurarDataGridView();
        }

        private void ConfigurarDataGridView()
        {
            // Primero asegurarse de que el DataGridView tiene datos
            if (dgvVehiculos.Columns.Count == 0)
            {
                MessageBox.Show("El DataGridView no tiene columnas cargadas aún.");
                return;
            }

            // Lista de columnas a ocultar (verifica los nombres exactos)
            var columnasAOcultar = new List<string>
    {
        "IdVehiculo",
        "id_vehiculo",
        "IdMarca",
        "id_marca",
        "IdModelo",
        "id_modelo",
        "marca",    // Por si acaso
        "modelo"     // Por si acaso
    };

            // Ocultar columnas que existan
            foreach (string columna in columnasAOcultar)
            {
                if (dgvVehiculos.Columns.Contains(columna))
                {
                    dgvVehiculos.Columns[columna].Visible = false;
                }
            }

            // Configurar nombres de columnas visibles
            dgvVehiculos.Columns["Placa"].HeaderText = "Placa";
            dgvVehiculos.Columns["Color"].HeaderText = "Color";
            dgvVehiculos.Columns["TipoVehiculo"].HeaderText = "Tipo";
            dgvVehiculos.Columns["PrecioDiario"].HeaderText = "Precio Diario";
            dgvVehiculos.Columns["Estado"].HeaderText = "Estado";
            dgvVehiculos.Columns["Kilometraje"].HeaderText = "Kilometraje";

            // Configurar formato numérico
            dgvVehiculos.Columns["PrecioDiario"].DefaultCellStyle.Format = "N2";
            dgvVehiculos.Columns["Kilometraje"].DefaultCellStyle.Format = "N0";

            // Autoajustar columnas
            dgvVehiculos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarMarcas()
        {
            try
            {
                var marcas = _marcaRepository.GetAllActive();
                cmbMarca.DataSource = marcas;
                cmbMarca.DisplayMember = "NombreMarca";
                cmbMarca.ValueMember = "IdMarca";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar marcas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarModelos(int marcaId)
        {
            try
            {
                var modelos = _modeloRepository.GetByMarca(marcaId);
                cmbModelo.DataSource = modelos;
                cmbModelo.DisplayMember = "NombreModelo";
                cmbModelo.ValueMember = "IdModelo";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar modelos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarVehiculos()
        {
            try
            {
                var vehiculos = _vehiculoRepository.GetAll();
                dgvVehiculos.DataSource = vehiculos;

                // Volver a configurar el DataGridView después de cargar datos
                ConfigurarDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar vehículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            _currentId = 0;
            txtAño.Text = string.Empty;
            txtChasis.Text = string.Empty;
            txtPlaca.Text = string.Empty;
            txtColor.Text = string.Empty;
            txtTipoVehiculo.Text = string.Empty;
            txtPrecioDiario.Text = string.Empty;
            txtKilometraje.Text = string.Empty;
            cmbEstado.SelectedIndex = 0;
            btnGuardar.Text = "Guardar";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var vehiculo = new Vehiculo
            {
                IdVehiculo = _currentId,
                IdMarca = (int)cmbMarca.SelectedValue,
                IdModelo = (int)cmbModelo.SelectedValue,
                Año = int.Parse(txtAño.Text),
                Chasis = txtChasis.Text,
                Placa = txtPlaca.Text,
                Color = txtColor.Text,
                TipoVehiculo = txtTipoVehiculo.Text,
                PrecioDiario = decimal.Parse(txtPrecioDiario.Text),
                Estado = cmbEstado.Text,
                Kilometraje = int.Parse(txtKilometraje.Text)
            };

            try
            {
                if (_currentId == 0)
                {
                    if (_vehiculoRepository.ExistsByChasisOrPlaca(vehiculo.Chasis, vehiculo.Placa))
                    {
                        MessageBox.Show("Ya existe un vehículo con este chasis o placa", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _vehiculoRepository.Add(vehiculo);
                    MessageBox.Show("Vehículo agregado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _vehiculoRepository.Update(vehiculo);
                    MessageBox.Show("Vehículo actualizado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarVehiculos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar vehículo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarCampos()
        {
            if (cmbMarca.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una marca", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbModelo.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un modelo", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAño.Text) || !int.TryParse(txtAño.Text, out _))
            {
                MessageBox.Show("Ingrese un año válido", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtChasis.Text))
            {
                MessageBox.Show("Ingrese el chasis del vehículo", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPlaca.Text))
            {
                MessageBox.Show("Ingrese la placa del vehículo", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtColor.Text))
            {
                MessageBox.Show("Ingrese el color del vehículo", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTipoVehiculo.Text))
            {
                MessageBox.Show("Ingrese el tipo de vehículo", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrecioDiario.Text) || !decimal.TryParse(txtPrecioDiario.Text, out _))
            {
                MessageBox.Show("Ingrese un precio diario válido", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtKilometraje.Text) || !int.TryParse(txtKilometraje.Text, out _))
            {
                MessageBox.Show("Ingrese un kilometraje válido", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void dgvVehiculos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvVehiculos.Rows[e.RowIndex];
                _currentId = Convert.ToInt32(row.Cells["IdVehiculo"].Value);

                var vehiculo = _vehiculoRepository.GetById(_currentId);
                if (vehiculo != null)
                {
                    cmbMarca.SelectedValue = vehiculo.IdMarca;
                    CargarModelos(vehiculo.IdMarca);
                    cmbModelo.SelectedValue = vehiculo.IdModelo;
                    txtAño.Text = vehiculo.Año.ToString();
                    txtChasis.Text = vehiculo.Chasis;
                    txtPlaca.Text = vehiculo.Placa;
                    txtColor.Text = vehiculo.Color;
                    txtTipoVehiculo.Text = vehiculo.TipoVehiculo;
                    txtPrecioDiario.Text = vehiculo.PrecioDiario.ToString();
                    cmbEstado.Text = vehiculo.Estado;
                    txtKilometraje.Text = vehiculo.Kilometraje.ToString();
                    btnGuardar.Text = "Actualizar";
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_currentId == 0)
            {
                MessageBox.Show("Seleccione un vehículo para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (MessageBox.Show("¿Está seguro que desea eliminar este vehículo?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_vehiculoRepository.Delete(_currentId))
                    {
                        MessageBox.Show("Vehículo eliminado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarVehiculos();
                        LimpiarCampos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMarca.SelectedValue != null && cmbMarca.SelectedValue is int marcaId)
            {
                CargarModelos(marcaId);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var vehiculos = _vehiculoRepository.GetByFilter(cmbFiltro.Text, txtBusqueda.Text);
            dgvVehiculos.DataSource = vehiculos;
            ConfigurarDataGridView();
        }
    }
}