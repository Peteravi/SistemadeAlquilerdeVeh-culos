using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Consultas
{
    public partial class frmConsultaReservasPorDias : Form
    {
        private readonly ReservaRepository _reservaRepository;

        public frmConsultaReservasPorDias()
        {
            InitializeComponent();
            _reservaRepository = new ReservaRepository();
            ConfigureGrid();
            LoadReservas();
        }

        private void ConfigureGrid()
        {
            dgvReservas.AutoGenerateColumns = false;
            dgvReservas.Columns.Clear();

            // Configuración de columnas
            var colId = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdReserva",
                HeaderText = "ID"
            };

            var colCliente = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreCliente",
                HeaderText = "Cliente"
            };

            var colVehiculo = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PlacaVehiculo",
                HeaderText = "Vehículo"
            };

            var colInicio = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaInicio",
                HeaderText = "Inicio",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            };

            var colFin = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaFin",
                HeaderText = "Fin",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            };

            // Columna para mostrar los días - ¡IMPORTANTE!
            var colDias = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DiasReserva", // Nombre exacto de la propiedad
                HeaderText = "Días",
                ValueType = typeof(int) // Especificar el tipo de dato
            };

            var colEstado = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Estado",
                HeaderText = "Estado"
            };

            dgvReservas.Columns.AddRange(colId, colCliente, colVehiculo, colInicio, colFin, colDias, colEstado);
        }

        private void LoadReservas(int? diasMin = null, int? diasMax = null)
        {
            try
            {
                var reservas = _reservaRepository.GetReservasPorFechas(
                    DateTime.Now.AddDays(-30),
                    DateTime.Now.AddDays(30));

                // Aplicar filtro por días si se especificó
                if (diasMin.HasValue || diasMax.HasValue)
                {
                    reservas = reservas.Where(r =>
                        (!diasMin.HasValue || r.DiasReserva >= diasMin.Value) &&
                        (!diasMax.HasValue || r.DiasReserva <= diasMax.Value))
                        .ToList();
                }

                dgvReservas.DataSource = reservas;
                lblTotal.Text = $"Total: {reservas.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtDiasMin.Clear();
            txtDiasMax.Clear();
            LoadReservas();
        }

        private void txtDias_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                int? diasMin = null;
                int? diasMax = null;

                // Validar y parsear los valores de los TextBox
                if (!string.IsNullOrWhiteSpace(txtDiasMin.Text))
                {
                    if (int.TryParse(txtDiasMin.Text, out int min))
                    {
                        diasMin = min;
                    }
                    else
                    {
                        MessageBox.Show("El valor mínimo de días debe ser un número válido",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtDiasMax.Text))
                {
                    if (int.TryParse(txtDiasMax.Text, out int max))
                    {
                        diasMax = max;
                    }
                    else
                    {
                        MessageBox.Show("El valor máximo de días debe ser un número válido",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Validar que el mínimo no sea mayor que el máximo
                if (diasMin.HasValue && diasMax.HasValue && diasMin > diasMax)
                {
                    MessageBox.Show("El número mínimo de días no puede ser mayor que el máximo",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Cargar reservas con los filtros aplicados
                LoadReservas(diasMin, diasMax);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al aplicar filtros: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}