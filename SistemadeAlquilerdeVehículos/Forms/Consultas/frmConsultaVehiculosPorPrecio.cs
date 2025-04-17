using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Consultas
{
    public partial class frmConsultaVehiculosPorPrecio : Form
    {
        private readonly VehiculoRepository _vehiculoRepository;
        private readonly OfertaRepository _ofertaRepository;

        public frmConsultaVehiculosPorPrecio()
        {
            InitializeComponent();
            _vehiculoRepository = new VehiculoRepository();
            _ofertaRepository = new OfertaRepository();
            CargarPreciosMinMax();
        }

        private void CargarPreciosMinMax()
        {
            try
            {
                decimal precioMin = _ofertaRepository.GetPrecioMinimoDisponible();
                decimal precioMax = _ofertaRepository.GetPrecioMaximoDisponible();

                txtPrecioMin.Text = precioMin.ToString("0.00");
                txtPrecioMax.Text = precioMax.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar precios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPrecioMin.Text, out decimal precioMin))
            {
                MessageBox.Show("Por favor ingrese un precio mínimo válido", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecioMax.Text, out decimal precioMax))
            {
                MessageBox.Show("Por favor ingrese un precio máximo válido", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (precioMin > precioMax)
            {
                MessageBox.Show("El precio mínimo no puede ser mayor que el precio máximo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var vehiculos = _vehiculoRepository.GetByPrecioRange(precioMin, precioMax);
                dgvVehiculos.DataSource = vehiculos;
                ConfigurarDataGridView();

                if (vehiculos.Count == 0)
                {
                    MessageBox.Show("No hay vehículos en el rango de precios especificado", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar vehículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvVehiculos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Configurar las columnas que quieres mostrar
            dgvVehiculos.Columns["IdVehiculo"].HeaderText = "ID";
            dgvVehiculos.Columns["NombreMarca"].HeaderText = "Marca";
            dgvVehiculos.Columns["NombreModelo"].HeaderText = "Modelo";
            dgvVehiculos.Columns["Año"].HeaderText = "Año";
            dgvVehiculos.Columns["Placa"].HeaderText = "Placa";
            dgvVehiculos.Columns["Color"].HeaderText = "Color";
            dgvVehiculos.Columns["TipoVehiculo"].HeaderText = "Tipo";
            dgvVehiculos.Columns["PrecioDiario"].HeaderText = "Precio Diario";
            dgvVehiculos.Columns["PrecioDiario"].DefaultCellStyle.Format = "C2";
            dgvVehiculos.Columns["Estado"].HeaderText = "Estado";
            dgvVehiculos.Columns["Kilometraje"].HeaderText = "Kilometraje";

            // Ocultar las columnas que no quieres mostrar
            dgvVehiculos.Columns["IdMarca"].Visible = false;
            dgvVehiculos.Columns["IdModelo"].Visible = false;
            dgvVehiculos.Columns["Chasis"].Visible = false;

            // Ocultar las columnas adicionales de Marca y Modelo que están al final
            foreach (DataGridViewColumn column in dgvVehiculos.Columns)
            {
                if (column.HeaderText == "Marca" && column.Index > dgvVehiculos.Columns["NombreMarca"].Index)
                {
                    column.Visible = false;
                }
                if (column.HeaderText == "Modelo" && column.Index > dgvVehiculos.Columns["NombreModelo"].Index)
                {
                    column.Visible = false;
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtPrecioMin.Clear();
            txtPrecioMax.Clear();
            dgvVehiculos.DataSource = null;
            CargarPreciosMinMax();
        }
    }
}