using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms
{
    public partial class frmSplash : Form
    {
        private System.Windows.Forms.Timer timer;
        private ProgressBar progressBar;
        private Label lblStatus;
        private Label lblVersion;
        private Label lblTitle;
        private PictureBox picLogo;
        private Panel panelMain;

        // Colores personalizados
        private readonly Color PrimaryColor = Color.FromArgb(0, 122, 204); // Azul profesional
        private readonly Color SecondaryColor = Color.FromArgb(30, 30, 35); // Fondo oscuro
        private readonly Color TextColor = Color.WhiteSmoke;

        public frmSplash()
        {
            InitializeComponents();
            ConfigureAppearance();
            StartLoadingAnimation();
        }

        private void InitializeComponents()
        {
            // Configuración básica del formulario
            this.ClientSize = new Size(700, 450);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = SecondaryColor;
            this.DoubleBuffered = true;

            // Panel principal con efecto de borde
            panelMain = new Panel
            {
                Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 40),
                Location = new Point(20, 20),
                BackColor = Color.FromArgb(45, 45, 50),
                Padding = new Padding(5)
            };

            // Título de la aplicación con efecto de gradiente
            lblTitle = new Label
            {
                Text = "SISTEMA DE ALQUILER DE VEHÍCULOS",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = false,
                Size = new Size(panelMain.Width - 20, 50),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(10, 30)
            };

            // Logo con efecto de brillo al cargar
            picLogo = new PictureBox
            {
                Size = new Size(180, 180),
                Location = new Point((panelMain.Width - 180) / 2, 90),
                BackColor = Color.Transparent,
                Image = CreateModernLogo(),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // Etiqueta de estado con efecto de transición
            lblStatus = new Label
            {
                Text = "Inicializando componentes principales...",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = TextColor,
                AutoSize = false,
                Size = new Size(panelMain.Width - 40, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 290)
            };

            // Barra de progreso con estilo moderno
            progressBar = new ProgressBar
            {
                Size = new Size(panelMain.Width - 60, 25),
                Location = new Point(30, 330),
                Style = ProgressBarStyle.Continuous,
                ForeColor = PrimaryColor,
                BackColor = Color.FromArgb(70, 70, 75)
            };

            // Versión con información de copyright
            lblVersion = new Label
            {
                Text = $"Versión {Application.ProductVersion} | © {DateTime.Now.Year} Alquiler de Vehículos",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.DarkGray,
                AutoSize = false,
                Size = new Size(panelMain.Width - 40, 20),
                TextAlign = ContentAlignment.MiddleRight,
                Location = new Point(20, panelMain.Height - 40)
            };

            // Agregar controles al panel principal
            panelMain.Controls.Add(lblTitle);
            panelMain.Controls.Add(picLogo);
            panelMain.Controls.Add(lblStatus);
            panelMain.Controls.Add(progressBar);
            panelMain.Controls.Add(lblVersion);

            // Agregar panel al formulario
            this.Controls.Add(panelMain);
        }

        private Image CreateModernLogo()
        {
            Bitmap bmp = new Bitmap(180, 180);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                // Círculo exterior con gradiente
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Rectangle(0, 0, 180, 180),
                    Color.FromArgb(0, 122, 204),
                    Color.FromArgb(0, 80, 160),
                    45f))
                {
                    g.FillEllipse(brush, 10, 10, 160, 160);
                }

                // Icono de vehículo (simplificado)
                using (Font iconFont = new Font("Wingdings", 48, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    g.DrawString("ö", iconFont, brush, new PointF(50, 50));
                }

                // Texto inferior
                using (Font font = new Font("Segoe UI", 14, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    g.DrawString("VEHÍCULOS", font, brush, new PointF(30, 130));
                }
            }
            return bmp;
        }

        private void ConfigureAppearance()
        {
            // Estilo moderno para la barra de progreso
            progressBar.SetState(2);

            // Efecto de sombra para el panel principal
            panelMain.Paint += (sender, e) => {
                ControlPaint.DrawBorder(e.Graphics, panelMain.ClientRectangle,
                    PrimaryColor, 2, ButtonBorderStyle.Solid,
                    PrimaryColor, 2, ButtonBorderStyle.Solid,
                    PrimaryColor, 2, ButtonBorderStyle.Solid,
                    PrimaryColor, 2, ButtonBorderStyle.Solid);
            };
        }

        private void StartLoadingAnimation()
        {
            timer = new System.Windows.Forms.Timer()
            {
                Interval = 50
            };
            timer.Tick += (s, e) => {
                if (progressBar.Value < progressBar.Maximum)
                {
                    progressBar.Value += 2;
                    UpdateStatusMessage();
                    AnimateLogo();
                }
                else
                {
                    timer.Stop();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };
            timer.Start();
        }

        private void AnimateLogo()
        {
            // Efecto de brillo intermitente en el logo
            float opacity = 0.7f + (float)Math.Sin(progressBar.Value * 0.1) * 0.3f;
            picLogo.Image = CreateModernLogoWithOpacity(opacity);
        }

        private Image CreateModernLogoWithOpacity(float opacity)
        {
            Bitmap bmp = new Bitmap(180, 180);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                // Círculo exterior con opacidad
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Rectangle(0, 0, 180, 180),
                    Color.FromArgb((int)(opacity * 255), 0, 122, 204),
                    Color.FromArgb((int)(opacity * 255), 0, 80, 160),
                    45f))
                {
                    g.FillEllipse(brush, 10, 10, 160, 160);
                }

                // Resto del logo con opacidad
                using (Font iconFont = new Font("Wingdings", 48, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(opacity * 255), 255, 255, 255)))
                {
                    g.DrawString("ö", iconFont, brush, new PointF(50, 50));
                }

                using (Font font = new Font("Segoe UI", 14, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(opacity * 255), 255, 255, 255)))
                {
                    g.DrawString("VEHÍCULOS", font, brush, new PointF(30, 130));
                }
            }
            return bmp;
        }

        private void UpdateStatusMessage()
        {
            string[] messages = {
                "Cargando módulo de vehículos...",
                "Inicializando sistema de clientes...",
                "Configurando base de datos...",
                "Preparando interfaz de usuario...",
                "Verificando conexiones...",
                "Cargando configuración inicial..."
            };

            // Cambiar mensaje basado en progreso
            int messageIndex = (progressBar.Value * messages.Length) / progressBar.Maximum;
            lblStatus.Text = messages[Math.Min(messageIndex, messages.Length - 1)];
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Dibujar sombra exterior
            using (var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
            {
                e.Graphics.FillRectangle(shadowBrush, 10, 10, this.Width - 20, this.Height - 20);
            }
        }
    }

    public static class ModifyProgressBarColor
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }
}