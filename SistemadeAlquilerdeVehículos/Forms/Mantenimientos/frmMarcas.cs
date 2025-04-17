using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Mantenimientos
{
    public partial class frmMarcas : Form
    {
        private readonly MarcaRepository _marcaRepository;
        private bool _isEditMode = false;
        private int _marcaId = 0;

        public frmMarcas()
        {
            InitializeComponent();
            _marcaRepository = new MarcaRepository();
        }

        private void frmMarcas_Load(object sender, EventArgs e)
        {
            LoadMarcas();
            ClearForm();
        }

        private void LoadMarcas()
        {
            dgvMarcas.DataSource = _marcaRepository.GetAll();
            dgvMarcas.Columns["IdMarca"].HeaderText = "ID";
            dgvMarcas.Columns["NombreMarca"].HeaderText = "Nombre de Marca";
            dgvMarcas.Columns["Estado"].HeaderText = "Estado";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            var marca = new Marca
            {
                NombreMarca = txtNombre.Text.Trim(),
                Estado = rbtActivo.Checked ? 'A' : 'I'
            };

            bool result;
            if (_isEditMode)
            {
                marca.IdMarca = _marcaId;
                result = _marcaRepository.Update(marca);
            }
            else
            {
                result = _marcaRepository.Create(marca);
            }

            if (result)
            {
                MessageBox.Show("Marca guardada correctamente", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMarcas();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Error al guardar la marca", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre de la marca es requerido", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            return true;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvMarcas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione una marca para editar", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _marcaId = Convert.ToInt32(dgvMarcas.SelectedRows[0].Cells["IdMarca"].Value);
            txtNombre.Text = dgvMarcas.SelectedRows[0].Cells["NombreMarca"].Value.ToString();
            var estado = dgvMarcas.SelectedRows[0].Cells["Estado"].Value.ToString();
            rbtActivo.Checked = estado == "A";
            rbtInactivo.Checked = estado != "A";
            _isEditMode = true;
            btnGuardar.Text = "Actualizar";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvMarcas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione una marca para eliminar", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro que desea eliminar esta marca?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var id = Convert.ToInt32(dgvMarcas.SelectedRows[0].Cells["IdMarca"].Value);
                var result = _marcaRepository.Delete(id);

                if (result)
                {
                    MessageBox.Show("Marca eliminada correctamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadMarcas();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Error al eliminar la marca", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtNombre.Clear();
            rbtActivo.Checked = true;
            _isEditMode = false;
            _marcaId = 0;
            btnGuardar.Text = "Guardar";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var nombre = txtBuscar.Text.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                LoadMarcas();
                return;
            }

            var marcas = _marcaRepository.GetAll();
            var filtered = marcas.Where(m => m.NombreMarca.ToLower().Contains(nombre.ToLower())).ToList();
            dgvMarcas.DataSource = filtered;
        }
    }
}