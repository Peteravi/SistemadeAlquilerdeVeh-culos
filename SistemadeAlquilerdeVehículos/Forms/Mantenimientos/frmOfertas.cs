using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Mantenimientos
{
    public partial class frmOfertas : Form
    {
        private readonly OfertaRepository _ofertaRepository;
        private readonly ModeloRepository _modeloRepository;
        private bool _esNuevo;

        public frmOfertas()
        {
            InitializeComponent();
            _ofertaRepository = new OfertaRepository();
            _modeloRepository = new ModeloRepository();
            _esNuevo = true;
        }

        public frmOfertas(int idOferta) : this()
        {
            _esNuevo = false;
            CargarOferta(idOferta);
        }

        private void frmOfertas_Load(object sender, EventArgs e)
        {
            CargarModelos();
            dtpLimiteOferta.MinDate = DateTime.Today;
        }

        private void CargarModelos()
        {
            try
            {
                cboModelo.DataSource = null;
                var modelos = _modeloRepository.ObtenerTodos();

                if (modelos == null || modelos.Count == 0)
                {
                    MessageBox.Show("No hay modelos disponibles para mostrar.",
                                  "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                cboModelo.DataSource = modelos;
                cboModelo.DisplayMember = "NombreModelo";
                cboModelo.ValueMember = "IdModelo";
                cboModelo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar modelos: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarOferta(int idOferta)
        {
            try
            {
                var oferta = _ofertaRepository.ObtenerPorId(idOferta);
                if (oferta != null)
                {
                    txtIdOferta.Text = oferta.IdOferta.ToString();
                    cboModelo.SelectedValue = oferta.IdModelo;
                    txtDescripcion.Text = oferta.DescripcionOferta;
                    txtPrecioOferta.Text = oferta.PrecioOferta.ToString("N2");
                    dtpLimiteOferta.Value = oferta.LimiteOferta;
                    cboModelo.Text = oferta.NombreModelo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar oferta: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboModelo.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un modelo de vehículo", "Validación",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var oferta = new Oferta
                {
                    IdModelo = Convert.ToInt32(cboModelo.SelectedValue),
                    DescripcionOferta = txtDescripcion.Text.Trim(),
                    PrecioOferta = decimal.Parse(txtPrecioOferta.Text),
                    LimiteOferta = dtpLimiteOferta.Value
                };

                if (!_esNuevo)
                {
                    oferta.IdOferta = Convert.ToInt32(txtIdOferta.Text);
                }

                if (!ValidarOferta(oferta))
                    return;

                if (_esNuevo)
                {
                    _ofertaRepository.Crear(oferta);
                    MessageBox.Show("Oferta creada exitosamente", "Éxito",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _ofertaRepository.Actualizar(oferta);
                    MessageBox.Show("Oferta actualizada exitosamente", "Éxito",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar oferta: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarOferta(Oferta oferta)
        {
            if (string.IsNullOrWhiteSpace(oferta.DescripcionOferta))
            {
                MessageBox.Show("La descripción de la oferta es requerida", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (oferta.PrecioOferta <= 0)
            {
                MessageBox.Show("El precio de oferta debe ser mayor que cero", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (oferta.LimiteOferta < DateTime.Today)
            {
                MessageBox.Show("La fecha límite no puede ser anterior al día actual", "Validación",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtPrecioOferta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}