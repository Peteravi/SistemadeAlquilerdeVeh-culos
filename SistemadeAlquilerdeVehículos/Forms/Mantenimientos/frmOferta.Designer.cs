namespace SistemadeAlquilerdeVehículos.Forms.Mantenimientos
{
    partial class frmOfertas
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.ComboBox cboModelo;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblPrecioOferta;
        private System.Windows.Forms.TextBox txtPrecioOferta;
        private System.Windows.Forms.Label lblLimiteOferta;
        private System.Windows.Forms.DateTimePicker dtpLimiteOferta;
        private System.Windows.Forms.Label lblPrecioNormal;
        private System.Windows.Forms.Label lblPrecioMinimo;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox txtIdOferta;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblModelo = new System.Windows.Forms.Label();
            this.cboModelo = new System.Windows.Forms.ComboBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblPrecioOferta = new System.Windows.Forms.Label();
            this.txtPrecioOferta = new System.Windows.Forms.TextBox();
            this.lblLimiteOferta = new System.Windows.Forms.Label();
            this.dtpLimiteOferta = new System.Windows.Forms.DateTimePicker();
            this.lblPrecioNormal = new System.Windows.Forms.Label();
            this.lblPrecioMinimo = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.txtIdOferta = new System.Windows.Forms.TextBox();
            this.panelHeader.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();

            // panelHeader
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(85)))), ((int)(((byte)(135)))));
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(500, 60);
            this.panelHeader.TabIndex = 0;

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(246, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Mantenimiento de Ofertas";

            // lblModelo
            this.lblModelo.AutoSize = true;
            this.lblModelo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModelo.Location = new System.Drawing.Point(30, 80);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Size = new System.Drawing.Size(122, 19);
            this.lblModelo.TabIndex = 1;
            this.lblModelo.Text = "Modelo del Vehículo:";

            // cboModelo
            this.cboModelo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboModelo.FormattingEnabled = true;
            this.cboModelo.Location = new System.Drawing.Point(30, 100);
            this.cboModelo.Name = "cboModelo";
            this.cboModelo.Size = new System.Drawing.Size(440, 25);
            this.cboModelo.TabIndex = 2;

            // lblDescripcion
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDescripcion.Location = new System.Drawing.Point(30, 140);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(137, 19);
            this.lblDescripcion.TabIndex = 3;
            this.lblDescripcion.Text = "Descripción de la Oferta:";

            // txtDescripcion
            this.txtDescripcion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescripcion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDescripcion.Location = new System.Drawing.Point(30, 160);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(440, 80);
            this.txtDescripcion.TabIndex = 4;

            // lblPrecioOferta
            this.lblPrecioOferta.AutoSize = true;
            this.lblPrecioOferta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPrecioOferta.Location = new System.Drawing.Point(30, 250);
            this.lblPrecioOferta.Name = "lblPrecioOferta";
            this.lblPrecioOferta.Size = new System.Drawing.Size(125, 19);
            this.lblPrecioOferta.TabIndex = 5;
            this.lblPrecioOferta.Text = "Precio de Oferta ($):";

            // txtPrecioOferta
            this.txtPrecioOferta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrecioOferta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPrecioOferta.Location = new System.Drawing.Point(30, 270);
            this.txtPrecioOferta.Name = "txtPrecioOferta";
            this.txtPrecioOferta.Size = new System.Drawing.Size(150, 25);
            this.txtPrecioOferta.TabIndex = 6;
            this.txtPrecioOferta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPrecioOferta.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPrecioOferta_KeyPress);

            // lblLimiteOferta
            this.lblLimiteOferta.AutoSize = true;
            this.lblLimiteOferta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLimiteOferta.Location = new System.Drawing.Point(200, 250);
            this.lblLimiteOferta.Name = "lblLimiteOferta";
            this.lblLimiteOferta.Size = new System.Drawing.Size(146, 19);
            this.lblLimiteOferta.TabIndex = 7;
            this.lblLimiteOferta.Text = "Fecha Límite de la Oferta:";

            // dtpLimiteOferta
            this.dtpLimiteOferta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpLimiteOferta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpLimiteOferta.Location = new System.Drawing.Point(200, 270);
            this.dtpLimiteOferta.Name = "dtpLimiteOferta";
            this.dtpLimiteOferta.Size = new System.Drawing.Size(150, 25);
            this.dtpLimiteOferta.TabIndex = 8;

            // lblPrecioNormal
            this.lblPrecioNormal.AutoSize = true;
            this.lblPrecioNormal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPrecioNormal.Location = new System.Drawing.Point(30, 310);
            this.lblPrecioNormal.Name = "lblPrecioNormal";
            this.lblPrecioNormal.Size = new System.Drawing.Size(104, 19);
            this.lblPrecioNormal.TabIndex = 9;
            this.lblPrecioNormal.Text = "Precio normal: $0";

            // lblPrecioMinimo
            this.lblPrecioMinimo.AutoSize = true;
            this.lblPrecioMinimo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPrecioMinimo.Location = new System.Drawing.Point(200, 310);
            this.lblPrecioMinimo.Name = "lblPrecioMinimo";
            this.lblPrecioMinimo.Size = new System.Drawing.Size(111, 19);
            this.lblPrecioMinimo.TabIndex = 10;
            this.lblPrecioMinimo.Text = "Precio mínimo: $0";

            // panelFooter
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panelFooter.Controls.Add(this.btnCancelar);
            this.panelFooter.Controls.Add(this.btnGuardar);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 350);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(500, 60);
            this.panelFooter.TabIndex = 11;

            // btnCancelar
            this.btnCancelar.BackColor = System.Drawing.Color.White;
            this.btnCancelar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnCancelar.FlatAppearance.BorderSize = 1;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCancelar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnCancelar.Location = new System.Drawing.Point(380, 15);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(90, 30);
            this.btnCancelar.TabIndex = 13;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            // btnGuardar
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(85)))), ((int)(((byte)(135)))));
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(280, 15);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(90, 30);
            this.btnGuardar.TabIndex = 12;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);

            // txtIdOferta
            this.txtIdOferta.Location = new System.Drawing.Point(0, 0);
            this.txtIdOferta.Name = "txtIdOferta";
            this.txtIdOferta.Size = new System.Drawing.Size(100, 20);
            this.txtIdOferta.TabIndex = 12;
            this.txtIdOferta.Visible = false;

            // frmOfertas
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(500, 410);
            this.Controls.Add(this.txtIdOferta);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.lblPrecioMinimo);
            this.Controls.Add(this.lblPrecioNormal);
            this.Controls.Add(this.dtpLimiteOferta);
            this.Controls.Add(this.lblLimiteOferta);
            this.Controls.Add(this.txtPrecioOferta);
            this.Controls.Add(this.lblPrecioOferta);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.cboModelo);
            this.Controls.Add(this.lblModelo);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOfertas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de Alquiler - Ofertas";
            this.Load += new System.EventHandler(this.frmOfertas_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}