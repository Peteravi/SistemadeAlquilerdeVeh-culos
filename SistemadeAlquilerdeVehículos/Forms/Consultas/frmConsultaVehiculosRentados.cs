using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Consultas
{
    public partial class frmConsultaVehiculosRentados : Form
    {
        private readonly VehiculoRepository _vehiculoRepository;

        public frmConsultaVehiculosRentados()
        {
            InitializeComponent();
            _vehiculoRepository = new VehiculoRepository();
        }

        private void frmConsultaVehiculosRentados_Load(object sender, EventArgs e)
        {
            CargarVehiculosRentados();
        }

        private void CargarVehiculosRentados()
        {
            try
            {
                var vehiculos = _vehiculoRepository.GetRentados();
                dgvVehiculos.DataSource = vehiculos;
                ConfigurarDataGridView();

                if (vehiculos.Count == 0)
                {
                    MessageBox.Show("No hay vehículos rentados en este momento", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar vehículos rentados: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarVehiculosRentados();
        }
    }
}