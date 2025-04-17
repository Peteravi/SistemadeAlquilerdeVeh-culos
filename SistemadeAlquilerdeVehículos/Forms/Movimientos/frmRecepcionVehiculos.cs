using SistemadeAlquilerdeVehículos.Data.Models;
using SistemadeAlquilerdeVehículos.Data.Repositories;
using System;
using System.Data;
using System.Windows.Forms;

namespace SistemadeAlquilerdeVehículos.Forms.Movimientos
{
	public partial class frmRecepcionVehiculos :Form
    {
		private readonly RecepcionRepository _recepcionRepo;
		private readonly VehiculoRepository _vehiculoRepo;
		private readonly AlquilerRepository _alquilerRepo;
		private readonly ReservaRepository _reservaRepo;

		public frmRecepcionVehiculos()
		{
			InitializeComponent();
			_recepcionRepo = new RecepcionRepository();
			_vehiculoRepo = new VehiculoRepository();
			_alquilerRepo = new AlquilerRepository();
			_reservaRepo = new ReservaRepository();
		}


		private void frmRecepcionVehiculos_Load(object sender, EventArgs e)
		{
			CargarVehiculos();
			dtpFechaRecepcion.Value = DateTime.Now;
			ConfigurarControles();
			CargarRecepciones();
		}
		private void ConfigurarControles()
		{
			// Configurar DataGridView
			dgvRecepciones.AutoGenerateColumns = false;
			dgvRecepciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgvRecepciones.MultiSelect = false;
			dgvRecepciones.AllowUserToAddRows = false;
			dgvRecepciones.AllowUserToDeleteRows = false;
			dgvRecepciones.ReadOnly = true;

			// Configurar columnas
			var colId = new DataGridViewTextBoxColumn
			{
				DataPropertyName = "IdRecepcion",
				HeaderText = "ID",
				Width = 50
			};

			var colPlaca = new DataGridViewTextBoxColumn
			{
				DataPropertyName = "Placa",
				HeaderText = "Placa",
				Width = 80
			};

			var colFecha = new DataGridViewTextBoxColumn
			{
				DataPropertyName = "FechaRecepcion",
				HeaderText = "Fecha Recepción",
				Width = 100,
				DefaultCellStyle = { Format = "dd/MM/yyyy" }
			};

			var colObservacion = new DataGridViewTextBoxColumn
			{
				DataPropertyName = "Observacion",
				HeaderText = "Observación",
				AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
			};

			dgvRecepciones.Columns.AddRange(colId, colPlaca, colFecha, colObservacion);
		}

		private void CargarVehiculos()
		{
			try
			{
				// Cambiar a GetAll() para todos los vehículos o mantener GetVehiculosNoDisponibles() si es el comportamiento deseado
				var vehiculos = _vehiculoRepo.GetAll();
				cmbVehiculo.DataSource = vehiculos;
				cmbVehiculo.DisplayMember = "Placa";
				cmbVehiculo.ValueMember = "IdVehiculo";
			}
			catch (Exception ex)
			{
				MostrarError($"Error al cargar vehículos: {ex.Message}");
			}
		}

		private void CargarRecepciones()
		{
			try
			{
				var recepciones = _recepcionRepo.GetAllWithVehiculoInfo();
				dgvRecepciones.DataSource = recepciones;
			}
			catch (Exception ex)
			{
				MostrarError($"Error al cargar recepciones: {ex.Message}");
			}
		}

		private void cmbVehiculo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbVehiculo.SelectedValue != null && cmbVehiculo.SelectedValue is int idVehiculo)
			{
				CargarInformacionVehiculo(idVehiculo);
			}
		}

		private void CargarInformacionVehiculo(int idVehiculo)
		{
			try
			{
				var vehiculo = _vehiculoRepo.GetById(idVehiculo);
				var ultimoAlquiler = _alquilerRepo.GetUltimoAlquilerPorVehiculo(idVehiculo);
				var ultimaReserva = _reservaRepo.GetUltimaReservaPorVehiculo(idVehiculo);

				string info = $"Estado actual: {vehiculo.Estado}\n";

				if (ultimoAlquiler != null)
				{
					info += $"Último alquiler: {ultimoAlquiler.FechaInicio:dd/MM/yyyy} - {ultimoAlquiler.FechaFin:dd/MM/yyyy}\n";
					info += $"Estado: {ultimoAlquiler.Estado}\n";
				}

				if (ultimaReserva != null)
				{
					info += $"Última reserva: {ultimaReserva.FechaInicio:dd/MM/yyyy} - {ultimaReserva.FechaFin:dd/MM/yyyy}\n";
					info += $"Estado: {ultimaReserva.Estado}";
				}

				txtInfoVehiculo.Text = info;
			}
			catch (Exception ex)
			{
				MostrarError($"Error al cargar información del vehículo: {ex.Message}");
			}
		}

		private void btnGuardar_Click(object sender, EventArgs e)
		{
			if (!ValidarDatos())
				return;

			try
			{
				var recepcion = new RecepcionVehiculo
				{
					IdVehiculo = (int)cmbVehiculo.SelectedValue,
					FechaRecepcion = dtpFechaRecepcion.Value,
					Observacion = txtObservacion.Text
				};

				int idRecepcion = _recepcionRepo.Create(recepcion);

				// Actualizar estado del vehículo a disponible
				var vehiculo = _vehiculoRepo.GetById(recepcion.IdVehiculo);
				if (vehiculo == null)
				{
					throw new Exception("No se encontró el vehículo");
				}

				vehiculo.Estado = "disponible";
				bool actualizado = _vehiculoRepo.Update(vehiculo);

				if (!actualizado)
				{
					throw new Exception("No se pudo actualizar el estado del vehículo");
				}

				string mensaje = $"Recepción registrada correctamente con ID: {idRecepcion}\n\n";
				mensaje += $"Vehículo: {cmbVehiculo.Text}\n";
				mensaje += $"Fecha de Recepción: {recepcion.FechaRecepcion:dd/MM/yyyy}\n";
				mensaje += $"Estado actualizado a: disponible";

				MostrarExito(mensaje);
				LimpiarFormulario();
				CargarRecepciones();
				CargarVehiculos();
			}
			catch (Exception ex)
			{
				MostrarError($"Error al guardar la recepción: {ex.Message}");
			}
		}

		private bool ValidarDatos()
		{
			if (cmbVehiculo.SelectedValue == null)
			{
				MostrarAdvertencia("Debe seleccionar un vehículo");
				cmbVehiculo.Focus();
				return false;
			}

			if (dtpFechaRecepcion.Value.Date > DateTime.Now.Date)
			{
				MostrarAdvertencia("La fecha de recepción no puede ser futura");
				dtpFechaRecepcion.Focus();
				return false;
			}

			if (string.IsNullOrWhiteSpace(txtObservacion.Text))
			{
				MostrarAdvertencia("Por favor ingrese una observación");
				txtObservacion.Focus();
				return false;
			}

			return true;
		}

		private void LimpiarFormulario()
		{
			cmbVehiculo.SelectedIndex = -1;
			dtpFechaRecepcion.Value = DateTime.Now;
			txtObservacion.Clear();
			txtInfoVehiculo.Clear();
		}

		private void btnBuscar_Click(object sender, EventArgs e)
		{
			try
			{
				// Crear un formulario de búsqueda simple o usar un InputBox
				string criterio = Microsoft.VisualBasic.Interaction.InputBox(
					"Ingrese placa o parte de la placa del vehículo:",
					"Buscar Vehículo",
					"",
					-1, -1);

				if (!string.IsNullOrWhiteSpace(criterio))
				{
					var vehiculos = _vehiculoRepo.GetByPlaca(criterio);

					if (vehiculos.Count == 0)
					{
						MostrarAdvertencia("No se encontraron vehículos con ese criterio de búsqueda");
						return;
					}

					// Mostrar resultados en un DataGridView o actualizar el ComboBox
					cmbVehiculo.DataSource = vehiculos;
					cmbVehiculo.DisplayMember = "Placa";
					cmbVehiculo.ValueMember = "IdVehiculo";

					if (vehiculos.Count == 1)
					{
						cmbVehiculo.SelectedIndex = 0;
						CargarInformacionVehiculo(vehiculos[0].IdVehiculo);
					}
				}
			}
			catch (Exception ex)
			{
				MostrarError($"Error al buscar vehículo: {ex.Message}");
			}
		}

		private void btnCancelar_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnLimpiar_Click(object sender, EventArgs e)
		{
			LimpiarFormulario();
		}

		private void MostrarError(string mensaje)
		{
			MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void MostrarAdvertencia(string mensaje)
		{
			MessageBox.Show(mensaje, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		private void MostrarExito(string mensaje)
		{
			MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}