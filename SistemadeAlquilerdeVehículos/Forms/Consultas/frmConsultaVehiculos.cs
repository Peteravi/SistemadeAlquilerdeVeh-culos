using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Consultas
{
    public partial class frmConsultaVehiculos : Form
    {
        private readonly VehiculoRepository _vehiculoRepository;

        public frmConsultaVehiculos()
        {
            InitializeComponent();
            _vehiculoRepository = new VehiculoRepository();
        }

        private void frmConsultaVehiculos_Load(object sender, EventArgs e)
        {
            CargarVehiculos();
        }

        private void CargarVehiculos()
        {
            try
            {
                var vehiculos = _vehiculoRepository.GetAllWithMarcaModelo();
                dgvVehiculos.DataSource = vehiculos;
                ConfigurarDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar vehículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim();
            if (string.IsNullOrEmpty(filtro))
            {
                CargarVehiculos();
                return;
            }

            try
            {
                var vehiculos = _vehiculoRepository.GetByFilter("placa", filtro)
                    .Select(v => new VehiculoDTO
                    {
                        IdVehiculo = v.IdVehiculo,
                        IdMarca = v.IdMarca,
                        IdModelo = v.IdModelo,
                        Año = v.Año,
                        Chasis = v.Chasis,
                        Placa = v.Placa,
                        Color = v.Color,
                        TipoVehiculo = v.TipoVehiculo,
                        PrecioDiario = v.PrecioDiario,
                        Estado = v.Estado,
                        Kilometraje = v.Kilometraje,
                        NombreMarca = "", // Estos se llenarán en otro método
                        NombreModelo = ""
                    }).ToList();

                dgvVehiculos.DataSource = vehiculos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar vehículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            CargarVehiculos();
        }
    }
}