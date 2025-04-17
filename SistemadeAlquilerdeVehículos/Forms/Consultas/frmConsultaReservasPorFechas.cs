using System;
using System.Drawing;
using System.Windows.Forms;
using SistemadeAlquilerdeVehículos.Data.Repositories;

namespace SistemadeAlquilerdeVehículos.Forms.Consultas
{
    public partial class frmConsultaReservasPorFechas : Form
    {
        private readonly ReservaRepository _repository;
        private const string ConnectionString = "Server=localhost;Database=alquiler_vehiculos;User Id=postgres;Password=1234;";

        public frmConsultaReservasPorFechas()
        {
            InitializeComponent();
            _repository = new ReservaRepository(ConnectionString);
            dtpFechaInicio.Value = new DateTime(2023, 11, 1);
            dtpFechaFin.Value = new DateTime(2023, 11, 10);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _repository?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ConfigurarDataGridView()
        {
            dgvReservas.AutoGenerateColumns = false;
            dgvReservas.Columns.Clear();

            // Configurar columnas manualmente
            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdReserva", // Cambiado a mayúsculas
                HeaderText = "ID Reserva",
                Width = 80
            };
            dgvReservas.Columns.Add(colId);

            DataGridViewTextBoxColumn colDoc = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NumDocumento", // Cambiado a mayúsculas
                HeaderText = "Documento"
            };
            dgvReservas.Columns.Add(colDoc);

            DataGridViewTextBoxColumn colFecReserva = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaReserva", // Cambiado a mayúsculas
                HeaderText = "Fecha Reserva",
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            };
            dgvReservas.Columns.Add(colFecReserva);

            DataGridViewTextBoxColumn colFecInicio = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaInicio", // Cambiado a mayúsculas
                HeaderText = "Fecha Inicio",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            };
            dgvReservas.Columns.Add(colFecInicio);

            DataGridViewTextBoxColumn colFecFin = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaFin", // Cambiado a mayúsculas
                HeaderText = "Fecha Fin",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            };
            dgvReservas.Columns.Add(colFecFin);

            DataGridViewTextBoxColumn colEstado = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Estado",
                HeaderText = "Estado"
            };
            dgvReservas.Columns.Add(colEstado);

            // Mejorar apariencia
            dgvReservas.EnableHeadersVisualStyles = false;
            dgvReservas.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvReservas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReservas.ColumnHeadersDefaultCellStyle.Font = new Font(dgvReservas.Font, FontStyle.Bold);
            dgvReservas.RowHeadersVisible = false;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = dtpFechaInicio.Value.Date;
                DateTime fechaFin = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1);

                if (fechaInicio > fechaFin)
                {
                    MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha fin",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var reservas = _repository.GetReservasPorFechas(fechaInicio, fechaFin);

                dgvReservas.DataSource = reservas;
                ConfigurarDataGridView();
                lblTotal.Text = $"Total de reservas: {reservas.Count}";

                if (reservas.Count == 0)
                {
                    MessageBox.Show("No se encontraron reservas para el rango de fechas seleccionado",
                                  "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar reservas: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dtpFechaInicio.Value = new DateTime(2023, 11, 1);
            dtpFechaFin.Value = new DateTime(2023, 11, 10);
            dgvReservas.DataSource = null;
            lblTotal.Text = "Total de reservas: 0";
        }
    }
}