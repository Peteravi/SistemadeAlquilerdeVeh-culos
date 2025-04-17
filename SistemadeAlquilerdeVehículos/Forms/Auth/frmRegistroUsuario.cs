using System;
using System.Windows.Forms;
using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;

namespace SistemadeAlquilerdeVehículos.Forms.Auth
{
    public partial class frmRegistroUsuario : Form
    {
        private readonly UsuarioRepository _usuarioRepository;

        public frmRegistroUsuario()
        {
            InitializeComponent();
            _usuarioRepository = new UsuarioRepository();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.AcceptButton = btnGuardar;
            this.CancelButton = btnCancelar;

            // Select the "Usuario" radio button by default
            rbUsuario.Checked = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                GuardarUsuario();
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellidos.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtPassword.Text != txtConfirmarPassword.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void GuardarUsuario()
        {
            try
            {
                var usuario = new Usuario
                {
                    LoginUsuario = txtLogin.Text.Trim(),
                    NombreUsuario = txtNombre.Text.Trim(),
                    ApellidosUsuario = txtApellidos.Text.Trim(),
                    NivelAcceso = rbAdmin.Checked ? 0 : 1, // 0 if Admin is checked, 1 otherwise
                    EmailUsuario = "" // Since there's no email field in your form
                };

                if (_usuarioRepository.RegistrarUsuario(usuario, txtPassword.Text))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar usuario: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}