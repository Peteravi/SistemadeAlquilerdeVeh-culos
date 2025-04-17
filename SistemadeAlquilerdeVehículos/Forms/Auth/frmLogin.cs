using System;
using System.Windows.Forms;
using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;

namespace SistemadeAlquilerdeVehículos.Forms.Auth
{
    public partial class frmLogin : Form
    {
        private readonly UsuarioRepository _usuarioRepository;
        private bool _credencialesValidas = false;

        public string NombreUsuarioCompleto { get; private set; }
        public int NivelAcceso { get; private set; }

        public frmLogin()
        {
            InitializeComponent();
            _usuarioRepository = new UsuarioRepository();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.AcceptButton = btnLogin;
            this.CancelButton = btnCancelar;

            // Configuración inicial de controles
            btnRegistrar.Visible = false;
            btnLogin.Enabled = false;

            // Enfoque inicial
            txtLogin.Focus();
        }

        private void txtLogin_TextChanged(object sender, EventArgs e)
        {
            ValidarCampos();
            btnRegistrar.Visible = txtLogin.Text.ToLower() == "admin";
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            ValidarCampos();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ValidarCredenciales();
            }
        }

        private void ValidarCampos()
        {
            btnLogin.Enabled = !string.IsNullOrWhiteSpace(txtLogin.Text) &&
                              !string.IsNullOrWhiteSpace(txtPassword.Text);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ValidarCredenciales();
        }

        private void ValidarCredenciales()
        {
            try
            {
                var usuario = _usuarioRepository.ObtenerPorCredenciales(txtLogin.Text.Trim(), txtPassword.Text);

                if (usuario != null)
                {
                    _credencialesValidas = true;
                    NombreUsuarioCompleto = $"{usuario.NombreUsuario} {usuario.ApellidosUsuario}";
                    NivelAcceso = usuario.NivelAcceso;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MostrarError("Credenciales incorrectas");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al validar credenciales: {ex.Message}");
            }
        }

        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            txtPassword.Clear();
            txtLogin.Focus();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            AbrirFormularioRegistro();
        }

        private void AbrirFormularioRegistro()
        {
            this.Hide();

            using (var frmRegistro = new frmRegistroUsuario())
            {
                if (frmRegistro.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Usuario registrado exitosamente", "Éxito",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            this.Show();
            txtLogin.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_credencialesValidas && this.DialogResult != DialogResult.OK)
            {
                Application.Exit();
            }
        }
    }
}