using SistemadeAlquilerdeVehículos.Data.Models;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Movimientos
{
    public partial class frmMostrarFactura : Form
    {
        private readonly Factura _factura;

        public frmMostrarFactura(Factura factura)
        {
            InitializeComponent();
            _factura = factura;
        }

        private void frmMostrarFactura_Load(object sender, EventArgs e)
        {
            // Configurar información básica
            lblNumeroFactura.Text = "Factura";
            lblFecha.Text = _factura.Fecha.ToString("dd/MM/yyyy");
            lblCliente.Text = _factura.Cliente.Nombre;
            lblDocumento.Text = $"{_factura.Cliente.TipoDocumento}: {_factura.Cliente.NumDocumento}";

            // Configurar DataGridView
            ConfigurarDataGridView();

            // Mostrar totales
            lblSubtotal.Text = _factura.Subtotal.ToString("C2");
            lblImpuesto.Text = _factura.Impuesto.ToString("C2");
            lblTotal.Text = _factura.TotalGeneral.ToString("C2");
        }

        private void ConfigurarDataGridView()
        {
            dgvDetalles.AutoGenerateColumns = false;
            dgvDetalles.DataSource = _factura.Detalles;

            // Configurar columnas
            if (dgvDetalles.Columns.Count == 0)
            {
                dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Descripción",
                    DataPropertyName = "Descripcion",
                    Width = 250
                });

                dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Días",
                    DataPropertyName = "CantidadDias",
                    Width = 60,
                    DefaultCellStyle = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleRight }
                });

                dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Precio Unitario",
                    DataPropertyName = "PrecioUnitario",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleRight,
                        Format = "C2"
                    }
                });

                dgvDetalles.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Total",
                    DataPropertyName = "Total",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Alignment = DataGridViewContentAlignment.MiddleRight,
                        Format = "C2"
                    }
                });
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Función de impresión habilitada", "Imprimir Factura",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}