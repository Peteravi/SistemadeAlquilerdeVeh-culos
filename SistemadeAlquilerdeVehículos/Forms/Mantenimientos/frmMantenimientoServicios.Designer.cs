namespace SistemadeAlquilerdeVehículos.Forms.Mantenimientos
{
    partial class frmMantenimientoServicios
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbDatos = new System.Windows.Forms.GroupBox();
            this.panelVehiculo = new System.Windows.Forms.Panel();
            this.labelVehiculoTitulo = new System.Windows.Forms.Label();
            this.lblPlaca = new System.Windows.Forms.Label();
            this.cmbVehiculo = new System.Windows.Forms.ComboBox();
            this.panelDescripcion = new System.Windows.Forms.Panel();
            this.labelDescripcionTitulo = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.panelDetalles = new System.Windows.Forms.Panel();
            this.txtCosto = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvMantenimientos = new System.Windows.Forms.DataGridView();
            this.panelBotones = new System.Windows.Forms.Panel();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.gbDatos.SuspendLayout();
            this.panelVehiculo.SuspendLayout();
            this.panelDescripcion.SuspendLayout();
            this.panelDetalles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMantenimientos)).BeginInit();
            this.panelBotones.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDatos
            // 
            this.gbDatos.BackColor = System.Drawing.Color.FromArgb(227, 242, 253);
            this.gbDatos.Controls.Add(this.panelVehiculo);
            this.gbDatos.Controls.Add(this.panelDescripcion);
            this.gbDatos.Controls.Add(this.panelDetalles);
            this.gbDatos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbDatos.ForeColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.gbDatos.Location = new System.Drawing.Point(12, 12);
            this.gbDatos.Name = "gbDatos";
            this.gbDatos.Size = new System.Drawing.Size(776, 220);
            this.gbDatos.TabIndex = 0;
            this.gbDatos.TabStop = false;
            this.gbDatos.Text = "Datos del Mantenimiento";
            // 
            // panelVehiculo
            // 
            this.panelVehiculo.BackColor = System.Drawing.Color.FromArgb(187, 222, 251);
            this.panelVehiculo.Controls.Add(this.labelVehiculoTitulo);
            this.panelVehiculo.Controls.Add(this.lblPlaca);
            this.panelVehiculo.Controls.Add(this.cmbVehiculo);
            this.panelVehiculo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelVehiculo.Location = new System.Drawing.Point(3, 19);
            this.panelVehiculo.Name = "panelVehiculo";
            this.panelVehiculo.Padding = new System.Windows.Forms.Padding(10);
            this.panelVehiculo.Size = new System.Drawing.Size(770, 60);
            this.panelVehiculo.TabIndex = 0;
            // 
            // labelVehiculoTitulo
            // 
            this.labelVehiculoTitulo.AutoSize = true;
            this.labelVehiculoTitulo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelVehiculoTitulo.ForeColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.labelVehiculoTitulo.Location = new System.Drawing.Point(13, 10);
            this.labelVehiculoTitulo.Name = "labelVehiculoTitulo";
            this.labelVehiculoTitulo.Size = new System.Drawing.Size(54, 15);
            this.labelVehiculoTitulo.TabIndex = 10;
            this.labelVehiculoTitulo.Text = "Vehículo:";
            // 
            // lblPlaca
            // 
            this.lblPlaca.AutoSize = true;
            this.lblPlaca.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPlaca.ForeColor = System.Drawing.Color.FromArgb(21, 101, 192);
            this.lblPlaca.Location = new System.Drawing.Point(73, 10);
            this.lblPlaca.Name = "lblPlaca";
            this.lblPlaca.Size = new System.Drawing.Size(0, 15);
            this.lblPlaca.TabIndex = 11;
            // 
            // cmbVehiculo
            // 
            this.cmbVehiculo.BackColor = System.Drawing.Color.White;
            this.cmbVehiculo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmbVehiculo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVehiculo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbVehiculo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbVehiculo.FormattingEnabled = true;
            this.cmbVehiculo.Location = new System.Drawing.Point(10, 35);
            this.cmbVehiculo.Name = "cmbVehiculo";
            this.cmbVehiculo.Size = new System.Drawing.Size(750, 23);
            this.cmbVehiculo.TabIndex = 1;
            this.cmbVehiculo.SelectedIndexChanged += new System.EventHandler(this.cmbVehiculo_SelectedIndexChanged);
            // 
            // panelDescripcion
            // 
            this.panelDescripcion.BackColor = System.Drawing.Color.FromArgb(207, 234, 252);
            this.panelDescripcion.Controls.Add(this.labelDescripcionTitulo);
            this.panelDescripcion.Controls.Add(this.txtDescripcion);
            this.panelDescripcion.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDescripcion.Location = new System.Drawing.Point(3, 79);
            this.panelDescripcion.Name = "panelDescripcion";
            this.panelDescripcion.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.panelDescripcion.Size = new System.Drawing.Size(770, 70);
            this.panelDescripcion.TabIndex = 1;
            // 
            // labelDescripcionTitulo
            // 
            this.labelDescripcionTitulo.AutoSize = true;
            this.labelDescripcionTitulo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelDescripcionTitulo.ForeColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.labelDescripcionTitulo.Location = new System.Drawing.Point(13, 5);
            this.labelDescripcionTitulo.Name = "labelDescripcionTitulo";
            this.labelDescripcionTitulo.Size = new System.Drawing.Size(72, 15);
            this.labelDescripcionTitulo.TabIndex = 12;
            this.labelDescripcionTitulo.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.BackColor = System.Drawing.Color.White;
            this.txtDescripcion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescripcion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtDescripcion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDescripcion.Location = new System.Drawing.Point(10, 25);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(750, 45);
            this.txtDescripcion.TabIndex = 3;
            // 
            // panelDetalles
            // 
            this.panelDetalles.BackColor = System.Drawing.Color.FromArgb(187, 222, 251);
            this.panelDetalles.Controls.Add(this.txtCosto);
            this.panelDetalles.Controls.Add(this.label5);
            this.panelDetalles.Controls.Add(this.dtpFechaFin);
            this.panelDetalles.Controls.Add(this.label4);
            this.panelDetalles.Controls.Add(this.dtpFechaInicio);
            this.panelDetalles.Controls.Add(this.label3);
            this.panelDetalles.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDetalles.Location = new System.Drawing.Point(3, 149);
            this.panelDetalles.Name = "panelDetalles";
            this.panelDetalles.Padding = new System.Windows.Forms.Padding(10);
            this.panelDetalles.Size = new System.Drawing.Size(770, 60);
            this.panelDetalles.TabIndex = 2;
            // 
            // txtCosto
            // 
            this.txtCosto.BackColor = System.Drawing.Color.White;
            this.txtCosto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCosto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCosto.Location = new System.Drawing.Point(533, 29);
            this.txtCosto.Name = "txtCosto";
            this.txtCosto.Size = new System.Drawing.Size(200, 23);
            this.txtCosto.TabIndex = 9;
            this.txtCosto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCosto_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.label5.Location = new System.Drawing.Point(530, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Costo";
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(270, 29);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(200, 23);
            this.dtpFechaFin.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.label4.Location = new System.Drawing.Point(267, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Fecha Fin";
            // 
            // dtpFechaInicio
            // 
            this.dtpFechaInicio.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaInicio.Location = new System.Drawing.Point(13, 29);
            this.dtpFechaInicio.Name = "dtpFechaInicio";
            this.dtpFechaInicio.Size = new System.Drawing.Size(200, 23);
            this.dtpFechaInicio.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.label3.Location = new System.Drawing.Point(10, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Fecha Inicio";
            // 
            // dgvMantenimientos
            // 
            this.dgvMantenimientos.AllowUserToAddRows = false;
            this.dgvMantenimientos.AllowUserToDeleteRows = false;
            this.dgvMantenimientos.AllowUserToResizeRows = false;
            this.dgvMantenimientos.BackgroundColor = System.Drawing.Color.White;
            this.dgvMantenimientos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMantenimientos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(13, 71, 161);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(13, 71, 161);
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMantenimientos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMantenimientos.ColumnHeadersHeight = 30;
            this.dgvMantenimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(33, 33, 33);
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(21, 101, 192);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMantenimientos.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMantenimientos.EnableHeadersVisualStyles = false;
            this.dgvMantenimientos.GridColor = System.Drawing.Color.FromArgb(207, 216, 220);
            this.dgvMantenimientos.Location = new System.Drawing.Point(12, 238);
            this.dgvMantenimientos.MultiSelect = false;
            this.dgvMantenimientos.Name = "dgvMantenimientos";
            this.dgvMantenimientos.ReadOnly = true;
            this.dgvMantenimientos.RowHeadersVisible = false;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5);
            this.dgvMantenimientos.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvMantenimientos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMantenimientos.Size = new System.Drawing.Size(776, 200);
            this.dgvMantenimientos.TabIndex = 1;
            this.dgvMantenimientos.SelectionChanged += new System.EventHandler(this.dgvMantenimientos_SelectionChanged);
            // 
            // panelBotones
            // 
            this.panelBotones.BackColor = System.Drawing.Color.FromArgb(227, 242, 253);
            this.panelBotones.Controls.Add(this.btnNuevo);
            this.panelBotones.Controls.Add(this.btnGuardar);
            this.panelBotones.Controls.Add(this.btnCancelar);
            this.panelBotones.Controls.Add(this.btnEliminar);
            this.panelBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBotones.Location = new System.Drawing.Point(0, 450);
            this.panelBotones.Name = "panelBotones";
            this.panelBotones.Size = new System.Drawing.Size(800, 50);
            this.panelBotones.TabIndex = 2;
            // 
            // btnNuevo
            // 
            this.btnNuevo.BackColor = System.Drawing.Color.FromArgb(21, 101, 192);
            this.btnNuevo.FlatAppearance.BorderSize = 0;
            this.btnNuevo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(13, 71, 161);
            this.btnNuevo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(25, 118, 210);
            this.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.Location = new System.Drawing.Point(12, 8);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(120, 35);
            this.btnNuevo.TabIndex = 2;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = false;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(56, 142, 60);
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(27, 94, 32);
            this.btnGuardar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(67, 160, 71);
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(142, 8);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(120, 35);
            this.btnGuardar.TabIndex = 3;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(183, 28, 28);
            this.btnCancelar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(229, 57, 53);
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(272, 8);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 35);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(211, 47, 47);
            this.btnEliminar.FlatAppearance.BorderSize = 0;
            this.btnEliminar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.btnEliminar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(229, 57, 53);
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.Location = new System.Drawing.Point(402, 8);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(120, 35);
            this.btnEliminar.TabIndex = 5;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // frmMantenimientoServicios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(236, 239, 241);
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.panelBotones);
            this.Controls.Add(this.dgvMantenimientos);
            this.Controls.Add(this.gbDatos);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMantenimientoServicios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mantenimiento de Servicios de Vehículos";
            this.Load += new System.EventHandler(this.frmMantenimientoServicios_Load);
            this.gbDatos.ResumeLayout(false);
            this.panelVehiculo.ResumeLayout(false);
            this.panelVehiculo.PerformLayout();
            this.panelDescripcion.ResumeLayout(false);
            this.panelDescripcion.PerformLayout();
            this.panelDetalles.ResumeLayout(false);
            this.panelDetalles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMantenimientos)).EndInit();
            this.panelBotones.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDatos;
        private System.Windows.Forms.Panel panelVehiculo;
        private System.Windows.Forms.Label labelVehiculoTitulo;
        private System.Windows.Forms.Label lblPlaca;
        private System.Windows.Forms.ComboBox cmbVehiculo;
        private System.Windows.Forms.Panel panelDescripcion;
        private System.Windows.Forms.Label labelDescripcionTitulo;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Panel panelDetalles;
        private System.Windows.Forms.TextBox txtCosto;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpFechaInicio;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvMantenimientos;
        private System.Windows.Forms.Panel panelBotones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnEliminar;
    }
}