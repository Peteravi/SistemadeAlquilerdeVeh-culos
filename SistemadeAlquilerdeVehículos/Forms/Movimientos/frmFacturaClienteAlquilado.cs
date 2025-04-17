using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Movimientos
{
    public partial class frmFacturaClienteAlquilado : Form
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly AlquilerRepository _alquilerRepository;

        public frmFacturaClienteAlquilado()
        {
            InitializeComponent();
            _clienteRepository = new ClienteRepository();
            _alquilerRepository = new AlquilerRepository();
        }

        private void frmFacturaClienteAlquilado_Load(object sender, EventArgs e)
        {
            CargarClientes();
            dtpFechaFactura.Value = DateTime.Today;
        }

        private void CargarClientes()
        {
            try
            {
                var clientes = _clienteRepository.ObtenerTodos();
                cmbClientes.DisplayMember = "Nombre";
                cmbClientes.ValueMember = "IdCliente";
                cmbClientes.DataSource = clientes;
                cmbClientes.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbClientes.SelectedIndex == -1) return;

            try
            {
                if (cmbClientes.SelectedItem is Cliente cliente)
                {
                    CargarAlquileresCliente(cliente.IdCliente);
                }
                else if (cmbClientes.SelectedValue != null && int.TryParse(cmbClientes.SelectedValue.ToString(), out int idCliente))
                {
                    CargarAlquileresCliente(idCliente);
                }
                else
                {
                    MessageBox.Show("No se pudo obtener el ID del cliente seleccionado",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la selección: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarAlquileresCliente(int idCliente)
        {
            try
            {
                var alquileres = _alquilerRepository.GetByCliente(idCliente);
                dgvAlquileres.DataSource = alquileres;

                if (dgvAlquileres.Columns.Count > 0)
                {
                    dgvAlquileres.Columns["IdAlquiler"].HeaderText = "ID Alquiler";
                    dgvAlquileres.Columns["IdCliente"].Visible = false;
                    dgvAlquileres.Columns["IdVehiculo"].Visible = false;
                    dgvAlquileres.Columns["FechaInicio"].HeaderText = "Fecha Inicio";
                    dgvAlquileres.Columns["FechaFin"].HeaderText = "Fecha Fin";
                    dgvAlquileres.Columns["TotalPago"].HeaderText = "Total a Pagar";
                    dgvAlquileres.Columns["Estado"].HeaderText = "Estado";

                    if (!dgvAlquileres.Columns.Contains("Seleccionar"))
                    {
                        DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn
                        {
                            Name = "Seleccionar",
                            HeaderText = "Seleccionar",
                            Width = 70
                        };
                        dgvAlquileres.Columns.Insert(0, checkColumn);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alquileres: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerarFactura_Click(object sender, EventArgs e)
        {
            if (cmbClientes.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un cliente", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!dgvAlquileres.Rows.Cast<DataGridViewRow>().Any(row => Convert.ToBoolean(row.Cells["Seleccionar"].Value)))
            {
                MessageBox.Show("Debe seleccionar al menos un alquiler para facturar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GenerarFactura();
        }

        private void GenerarFactura()
        {
            try
            {
                var cliente = (Cliente)cmbClientes.SelectedItem;
                var factura = new Factura
                {
                    Fecha = dtpFechaFactura.Value,
                    Cliente = cliente
                };

                foreach (DataGridViewRow row in dgvAlquileres.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["Seleccionar"].Value))
                    {
                        var idAlquiler = Convert.ToInt32(row.Cells["IdAlquiler"].Value);
                        var alquiler = _alquilerRepository.GetById(idAlquiler);
                        var dias = (alquiler.FechaFin - alquiler.FechaInicio).Days;

                        factura.Detalles.Add(new DetalleFactura
                        {
                            Alquiler = alquiler,
                            Descripcion = $"Alquiler de vehículo {alquiler.Vehiculo.Marca.NombreMarca} {alquiler.Vehiculo.Modelo.NombreModelo}",
                            CantidadDias = dias,
                            PrecioUnitario = dias > 0 ? alquiler.TotalPago / dias : alquiler.TotalPago,
                            Total = alquiler.TotalPago
                        });
                    }
                }

                factura.Impuesto = factura.Subtotal * 0.18m;

                using (var frm = new frmMostrarFactura(factura))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar factura: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbClientes.SelectedIndex = -1;
            dgvAlquileres.DataSource = null;
        }
    }
}