using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Data;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Consultas
{
	public partial class frmConsultaRecepcionPorFecha : Form
    {
		private readonly RecepcionRepository _recepcionRepository;

		public frmConsultaRecepcionPorFecha() : this(new RecepcionRepository())
		{
		}

		public frmConsultaRecepcionPorFecha(RecepcionRepository recepcionRepository)
		{
			InitializeComponent();
			_recepcionRepository = recepcionRepository;
			dtpFechaConsulta.Value = DateTime.Today;
		}

		private void btnBuscar_Click(object sender, EventArgs e)
		{
			try
			{
				DateTime fechaConsulta = dtpFechaConsulta.Value;
				DataTable resultados = _recepcionRepository.GetByDate(fechaConsulta);

				dgvResultados.DataSource = resultados;

				if (resultados.Rows.Count == 0)
				{
					MessageBox.Show("No se encontraron recepciones para la fecha seleccionada.",
								  "Información",
								  MessageBoxButtons.OK,
								  MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al consultar recepciones: {ex.Message}",
							  "Error",
							  MessageBoxButtons.OK,
							  MessageBoxIcon.Error);
			}
		}

		private void btnLimpiar_Click(object sender, EventArgs e)
		{
			dgvResultados.DataSource = null;
			dtpFechaConsulta.Value = DateTime.Today;
		}

		private void btnSalir_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}