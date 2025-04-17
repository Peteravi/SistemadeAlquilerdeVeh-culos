using System;
using System.Windows.Forms;
using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;

namespace SistemadeAlquilerdeVehículos.Forms.Mantenimientos
{
    public partial class frmUsuarios : Form
    {
        private readonly UsuarioRepository _usuarioRepository;
        private bool _modoEdicion = false;

        public frmUsuarios()
        {
            InitializeComponent();
            _usuarioRepository = new UsuarioRepository();
            ConfigurarEventos();
        }

        private void ConfigurarEventos()
        {
            dgvUsuarios.SelectionChanged += (s, e) => ActualizarEstadoBotones();
            this.Load += (s, e) => {
                CargarUsuarios();
                ConfigurarNivelAcceso();
                ConfigurarDataGridView();
                ActualizarEstadoBotones();
                HabilitarCampos(false);
            };
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            // La carga inicial se maneja en ConfigurarEventos
        }

        private void CargarUsuarios()
        {
            try
            {
                var usuarios = _usuarioRepository.ObtenerTodos();
                dgvUsuarios.DataSource = usuarios;
                dgvUsuarios.ClearSelection();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar usuarios: {ex.Message}");
            }
        }

        private void ConfigurarNivelAcceso()
        {
            cmbNivelAcceso.Items.Clear();
            cmbNivelAcceso.Items.AddRange(new object[] { "0 - Administrador", "1 - Usuario Normal" });
            cmbNivelAcceso.SelectedIndex = 1;
        }

        private void ConfigurarDataGridView()
        {
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.Columns["PassUsuario"].Visible = false;
            dgvUsuarios.Columns["LoginUsuario"].HeaderText = "Login";
            dgvUsuarios.Columns["NombreUsuario"].HeaderText = "Nombre";
            dgvUsuarios.Columns["ApellidosUsuario"].HeaderText = "Apellidos";
            dgvUsuarios.Columns["NivelAcceso"].HeaderText = "Nivel Acceso";
            dgvUsuarios.Columns["EmailUsuario"].HeaderText = "Email";
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            _modoEdicion = true;
            LimpiarCampos();
            HabilitarCampos(true);
            ActualizarEstadoBotones();
            txtLoginUsuario.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            try
            {
                var usuario = new Usuario
                {
                    LoginUsuario = txtLoginUsuario.Text.Trim(),
                    PassUsuario = txtPassword.Text,
                    NombreUsuario = txtNombre.Text.Trim(),
                    ApellidosUsuario = txtApellidos.Text.Trim(),
                    NivelAcceso = cmbNivelAcceso.SelectedIndex,
                    EmailUsuario = txtEmail.Text.Trim()
                };

                var usuarioExistente = _usuarioRepository.ObtenerPorLogin(txtLoginUsuario.Text.Trim());

                if (usuarioExistente == null)
                {
                    _usuarioRepository.Crear(usuario);
                    MostrarExito("Usuario creado exitosamente.");
                }
                else
                {
                    _usuarioRepository.Actualizar(usuario);
                    MostrarExito("Usuario actualizado exitosamente.");
                }

                _modoEdicion = false;
                CargarUsuarios();
                LimpiarCampos();
                HabilitarCampos(false);
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al guardar usuario: {ex.Message}");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _modoEdicion = false;
            LimpiarCampos();
            HabilitarCampos(false);
            ActualizarEstadoBotones();
            dgvUsuarios.ClearSelection();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MostrarAdvertencia("Por favor seleccione un usuario.");
                return;
            }

            var usuario = (Usuario)dgvUsuarios.SelectedRows[0].DataBoundItem;
            txtLoginUsuario.Text = usuario.LoginUsuario;
            txtPassword.Text = usuario.PassUsuario;
            txtNombre.Text = usuario.NombreUsuario;
            txtApellidos.Text = usuario.ApellidosUsuario;
            cmbNivelAcceso.SelectedIndex = usuario.NivelAcceso;
            txtEmail.Text = usuario.EmailUsuario;

            _modoEdicion = true;
            HabilitarCampos(true);
            txtLoginUsuario.Enabled = false;
            ActualizarEstadoBotones();
            txtNombre.Focus();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MostrarAdvertencia("Por favor seleccione un usuario.");
                return;
            }

            if (MessageBox.Show("¿Está seguro que desea eliminar este usuario?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var loginUsuario = dgvUsuarios.SelectedRows[0].Cells["LoginUsuario"].Value.ToString();
                    _usuarioRepository.Eliminar(loginUsuario);
                    MostrarExito("Usuario eliminado exitosamente.");
                    CargarUsuarios();
                    LimpiarCampos();
                    ActualizarEstadoBotones();
                }
                catch (Exception ex)
                {
                    MostrarError($"Error al eliminar usuario: {ex.Message}");
                }
            }
        }

        private void LimpiarCampos()
        {
            txtLoginUsuario.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellidos.Text = string.Empty;
            txtEmail.Text = string.Empty;
            cmbNivelAcceso.SelectedIndex = 1;
        }

        private void HabilitarCampos(bool habilitar)
        {
            txtLoginUsuario.Enabled = habilitar;
            txtPassword.Enabled = habilitar;
            txtNombre.Enabled = habilitar;
            txtApellidos.Enabled = habilitar;
            txtEmail.Enabled = habilitar;
            cmbNivelAcceso.Enabled = habilitar;
        }

        private void ActualizarEstadoBotones()
        {
            bool haySeleccion = dgvUsuarios.SelectedRows.Count > 0;

            btnNuevo.Enabled = !_modoEdicion;
            btnGuardar.Enabled = _modoEdicion;
            btnCancelar.Enabled = _modoEdicion;
            btnEditar.Enabled = !_modoEdicion && haySeleccion;
            btnEliminar.Enabled = !_modoEdicion && haySeleccion;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtLoginUsuario.Text))
            {
                MostrarAdvertencia("El login de usuario es obligatorio.");
                txtLoginUsuario.Focus();
                return false;
            }

            if (txtLoginUsuario.Text.Length < 4)
            {
                MostrarAdvertencia("El login debe tener al menos 4 caracteres.");
                txtLoginUsuario.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MostrarAdvertencia("La contraseña es obligatoria.");
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                MostrarAdvertencia("La contraseña debe tener al menos 6 caracteres.");
                txtPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MostrarAdvertencia("El nombre es obligatorio.");
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellidos.Text))
            {
                MostrarAdvertencia("Los apellidos son obligatorios.");
                txtApellidos.Focus();
                return false;
            }

            if (cmbNivelAcceso.SelectedIndex == -1)
            {
                MostrarAdvertencia("Debe seleccionar un nivel de acceso.");
                cmbNivelAcceso.Focus();
                return false;
            }

            return true;
        }

        private void txtLoginUsuario_Leave(object sender, EventArgs e)
        {
            if (_modoEdicion && !string.IsNullOrEmpty(txtLoginUsuario.Text))
            {
                var usuarioExistente = _usuarioRepository.ObtenerPorLogin(txtLoginUsuario.Text.Trim());
                if (usuarioExistente != null)
                {
                    MostrarAdvertencia("Este login de usuario ya existe.");
                    txtLoginUsuario.Focus();
                }
            }
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarEstadoBotones();
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