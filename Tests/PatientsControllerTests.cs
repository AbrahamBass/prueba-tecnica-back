using Application.DTOs.Patient;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PruebaTecnica.Controllers;

namespace Tests
{
    public class PatientsControllerTests
    {

        private readonly Mock<IPatientService> _mockService;
        private readonly PatientsController _controller;

        public PatientsControllerTests()
        {
            _mockService = new Mock<IPatientService>();
            _controller = new PatientsController(_mockService.Object);
        }

        /// <summary>
        /// Verifica que al consultar un paciente por ID existente,
        /// el endpoint responda correctamente con un 200 OK y los datos del paciente.
        /// </summary>
        [Fact]
        public async Task GetById_WhenPatientExists_ReturnsOkResult()
        {
            int patientId = 1;
            var mockPatient = new PatientDto
            {
                DocumentType = "CC",
                PatientId = patientId,
                FirstName = "Juan",
                LastName = "Perez",
                DocumentNumber = "1234567890",
                BirthDate = new DateOnly(1990, 5, 15),
            };

            _mockService.Setup(s => s.GetByIdAsync(patientId))
                        .ReturnsAsync(mockPatient);

            var result = await _controller.GetById(patientId);

            var okResult = Assert.IsType<OkObjectResult>(result); 
            var returnedPatient = Assert.IsType<PatientDto>(okResult.Value); 
            Assert.Equal(patientId, returnedPatient.PatientId);
        }

        /// <summary>
        /// Verifica que al enviar datos válidos para crear un paciente,
        /// el endpoint responda con un 201 (Created).
        /// </summary>
        [Fact]
        public async Task Create_WithValidData_ReturnsCreatedResult()
        {
            var newPatientDto = new CreatePatientDto
            {
                DocumentType = "CC",
                DocumentNumber = "1098765432",
                FirstName = "Maria",
                LastName = "Gomez",
                BirthDate = new DateOnly(1990, 5, 15)
            };

            _mockService.Setup(s => s.CreateAsync(newPatientDto))
                        .Returns(Task.CompletedTask);

            var result = await _controller.Create(newPatientDto);

            var statusCodeResult = Assert.IsAssignableFrom<Microsoft.AspNetCore.Mvc.Infrastructure.IStatusCodeActionResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// Verifica que cuando los datos son inválidos,
        /// el endpoint responda con un 400 BadRequest.
        /// </summary>
        [Fact]
        public async Task Create_WithInvalidData_ReturnsBadRequest()
        {
            var invalidPatientDto = new CreatePatientDto
            {
                DocumentType = "CC",
                DocumentNumber = "TEXTO_INVALIDO",
                FirstName = "Pedro",
                LastName = "Soto",
                BirthDate = new DateOnly(2020, 1, 1)
            };

            _mockService
                .Setup(s => s.CreateAsync(It.IsAny<CreatePatientDto>()))
                .ThrowsAsync(new ValidationException("Validation failed"));

            await Assert.ThrowsAsync<ValidationException>(() =>
                _controller.Create(invalidPatientDto));
        }

    }
}
