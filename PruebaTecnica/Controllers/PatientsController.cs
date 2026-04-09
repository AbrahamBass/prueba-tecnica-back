using Application.DTOs.Patient;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PruebaTecnica.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _service;
        public PatientsController(IPatientService service)
        {
            _service = service;
        }

        /// <summary>
        /// Crea un nuevo paciente en el sistema.
        /// </summary>
        /// <param name="body">Datos necesarios para crear el paciente.</param>
        /// <returns>201 si el paciente fue creado correctamente.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientDto body)
        {
            await _service.CreateAsync(body);
            return Created();
        }

        /// <summary>
        /// Obtiene una lista paginada de pacientes, con opción de filtrar por nombre o número de documento.
        /// </summary>
        /// <param name="page">Número de página.</param>
        /// <param name="pageSize">Cantidad de registros por página.</param>
        /// <param name="name">Filtro opcional por nombre.</param>
        /// <param name="documentNumber">Filtro opcional por número de documento.</param>
        /// <returns>Listado de pacientes.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? name = null,
            [FromQuery] string? documentNumber = null
        )
        {
            return Ok(await _service.GetAllAsync(page, pageSize, name, documentNumber));
        }


        /// <summary>
        /// Obtiene un paciente por su identificador.
        /// </summary>
        /// <param name="id">Id del paciente.</param>
        /// <returns>El paciente si existe, de lo contrario 404.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _service.GetByIdAsync(id);

            if (patient == null)
                return NotFound();

            return Ok(patient);
        }

        /// <summary>
        /// Actualiza la información de un paciente existente.
        /// </summary>
        /// <param name="id">Id del paciente a actualizar.</param>
        /// <param name="body">Datos a actualizar del paciente.</param>
        /// <returns>204 si la actualización fue exitosa.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto body)
        {
            await _service.UpdateAsync(id, body);
            return NoContent();
        }

        /// <summary>
        /// Actualiza la información de un paciente existente.
        /// </summary>
        /// <param name="id">Id del paciente a actualizar.</param>
        /// <param name="body">Datos a actualizar del paciente.</param>
        /// <returns>204 si la actualización fue exitosa.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }


        /// <summary>
        /// Obtiene los pacientes creados a partir de una fecha específica.
        /// </summary>
        /// <param name="fromDate">Fecha desde la cual se desean consultar los pacientes.</param>
        /// <returns>Listado de pacientes filtrados por fecha de creación.</returns>
        [HttpGet("report")]
        public async Task<IActionResult> GetPatientsCreatedAfter([FromQuery] DateTime fromDate)
        {
            var patients = await _service.GetPatientsCreatedAfterAsync(fromDate);
            return Ok(patients);
        }

    }
}
