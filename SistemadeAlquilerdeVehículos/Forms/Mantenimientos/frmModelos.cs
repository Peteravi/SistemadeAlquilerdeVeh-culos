using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Mantenimientos
{
    public partial class frmModelos : Form
    {
        private readonly ModeloRepository _modeloRepository;
        private Modelo _modeloActual;

        public frmModelos()
        {
            InitializeComponent();
            _modeloRepository = new ModeloRepository();
            _modeloActual = null;
            CargarModelos();
            ConfigurarDataGridView();
            ConfigurarComboEstado();
        }

        private void ConfigurarDataGridView()
        {
            dgvModelos.AutoGenerateColumns = false;
            dgvModelos.Columns.Clear();

            dgvModelos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "IdModelo",
                HeaderText = "ID",
                Name = "colId",
                ReadOnly = true,
                Width = 50
            });

            dgvModelos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "NombreModelo",
                HeaderText = "Nombre del Modelo",
                Name = "colNombre",
                ReadOnly = true,
                Width = 200
            });

            dgvModelos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Estado",
                HeaderText = "Estado",
                Name = "colEstado",
                ReadOnly = true,
                Width = 80
            });
        }

        private void ConfigurarComboEstado()
        {
            cmbEstado.Items.Add('A');
            cmbEstado.Items.Add('I');
            cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarModelos()
        {
            try
            {
                var modelos = _modeloRepository.GetAll();
                dgvModelos.DataSource = modelos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar modelos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            HabilitarControles(true);
            _modeloActual = null;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvModelos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un modelo para editar.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idModelo = Convert.ToInt32(dgvModelos.SelectedRows[0].Cells["colId"].Value);
            CargarModeloParaEditar(idModelo);
        }

        private void CargarModeloParaEditar(int idModelo)
        {
            try
            {
                _modeloActual = _modeloRepository.GetById(idModelo);
                if (_modeloActual != null)
                {
                    txtNombre.Text = _modeloActual.NombreModelo;
                    txtPrecio.Text = _modeloActual.Precio.ToString("N2");
                    cmbEstado.SelectedItem = _modeloActual.Estado;
                    HabilitarControles(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar modelo: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvModelos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un modelo para eliminar.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idModelo = Convert.ToInt32(dgvModelos.SelectedRows[0].Cells["colId"].Value);
            var nombreModelo = dgvModelos.SelectedRows[0].Cells["colNombre"].Value.ToString();

            if (MessageBox.Show($"¿Está seguro que desea eliminar el modelo '{nombreModelo}'?",
                "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (_modeloRepository.Delete(idModelo))
                    {
                        MessageBox.Show("Modelo eliminado correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarModelos();
                        LimpiarFormulario();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar modelo: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            try
            {
                var modelo = new Modelo
                {
                    NombreModelo = txtNombre.Text.Trim(),
                    Estado = (char)cmbEstado.SelectedItem,
                    Precio = decimal.Parse(txtPrecio.Text)
                };

                bool resultado;
                string mensaje;

                if (_modeloActual == null)
                {
                    if (_modeloRepository.Exists(modelo.NombreModelo))
                    {
                        MessageBox.Show("Ya existe un modelo con este nombre.",
                            "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    resultado = _modeloRepository.Add(modelo);
                    mensaje = resultado ? "Modelo agregado correctamente." : "No se pudo agregar el modelo.";
                }
                else
                {
                    modelo.IdModelo = _modeloActual.IdModelo;
                    if (_modeloRepository.Exists(modelo.NombreModelo, modelo.IdModelo))
                    {
                        MessageBox.Show("Ya existe otro modelo con este nombre.",
                            "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    resultado = _modeloRepository.Update(modelo);
                    mensaje = resultado ? "Modelo actualizado correctamente." : "No se pudo actualizar el modelo.";
                }

                if (resultado)
                {
                    MessageBox.Show(mensaje, "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarModelos();
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar modelo: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del modelo es requerido.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (txtNombre.Text.Length > 20)
            {
                MessageBox.Show("El nombre del modelo no puede exceder los 20 caracteres.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (cmbEstado.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un estado.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbEstado.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("El precio debe ser un valor numérico mayor que cero.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            cmbEstado.SelectedIndex = -1;
            HabilitarControles(false);
        }

        private void HabilitarControles(bool habilitar)
        {
            txtNombre.Enabled = habilitar;
            txtPrecio.Enabled = habilitar;
            cmbEstado.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            btnCancelar.Enabled = habilitar;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var modelos = _modeloRepository.GetAll()
                    .Where(m => m.NombreModelo.ToLower().Contains(txtBuscar.Text.ToLower()))
                    .ToList();

                dgvModelos.DataSource = modelos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar modelos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvModelos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvModelos.SelectedRows.Count > 0)
            {
                var idModelo = Convert.ToInt32(dgvModelos.SelectedRows[0].Cells["colId"].Value);
                CargarModeloParaEditar(idModelo);
            }
        }
    }
}