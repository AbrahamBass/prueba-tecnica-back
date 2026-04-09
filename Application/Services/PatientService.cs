using Application.DTOs.Common;
using Application.DTOs.Patient;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class PatientService : IPatientService
    {

        private readonly IPatientRepository _repository;
        private readonly IValidator<CreatePatientDto> _createValidator;
        private readonly IValidator<UpdatePatientDto> _updateValidator;

        public PatientService(
            IPatientRepository repository,
            IValidator<CreatePatientDto> createValidator,
            IValidator<UpdatePatientDto> updateValidator
        )
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<PagedResult<PatientDto>> GetAllAsync(
            int page,
            int pageSize,
            string? name,
            string? documentNumber
        )
        {
            var (patients, totalCount) = await _repository.GetPagedAsync(page, pageSize, name, documentNumber);

            return new PagedResult<PatientDto>
            {
                Items = patients.Select(p => new PatientDto
                {
                    BirthDate = p.BirthDate,
                    DocumentType = p.DocumentType,
                    PatientId = p.PatientId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    DocumentNumber = p.DocumentNumber,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    CreatedAt = p.CreatedAt
                }).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            
            if (patient == null)
            {
                throw new NotFoundException("Paciente no encontrado");
            }

            return new PatientDto
            {
                PatientId = patient.PatientId,
                DocumentType = patient.DocumentType,
                DocumentNumber = patient.DocumentNumber,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                CreatedAt = patient.CreatedAt
            };
        }

        public async Task CreateAsync(CreatePatientDto dto)
        {

            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var exists = await _repository.ExistsAsync(dto.DocumentType, dto.DocumentNumber);
            if (exists)
            {
                throw new BadRequestException("Ya existe un paciente registrado con ese tipo y número de documento.");
            }

            var patient = new Patients
            {
                DocumentType = dto.DocumentType,
                DocumentNumber = dto.DocumentNumber,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow
            };

            
            await _repository.AddAsync(patient);
            
        }

        public async Task UpdateAsync(int id, UpdatePatientDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) throw new KeyNotFoundException("Paciente no encontrado");

            if (patient.DocumentType != dto.DocumentType ||
                patient.DocumentNumber != dto.DocumentNumber)
            {
                var exists = await _repository.ExistsAsync(dto.DocumentType, dto.DocumentNumber);

                if (exists)
                {
                    throw new BadRequestException("Ya existe un paciente con ese documento.");
                }
                    
            }

            patient.DocumentType = dto.DocumentType;
            patient.DocumentNumber = dto.DocumentNumber;
            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.BirthDate = dto.BirthDate;
            patient.Email = dto.Email;
            patient.PhoneNumber = dto.PhoneNumber;

            _repository.Update(patient);
        }

        public async Task<IEnumerable<PatientDto>> GetPatientsCreatedAfterAsync(DateTime date)
        {
            var patients = await _repository.GetCreatedAfterAsync(date);

            return patients.Select(p => new PatientDto
            {
                PatientId = p.PatientId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                DocumentNumber = p.DocumentNumber,
                Email = p.Email,
                PhoneNumber = p.PhoneNumber,
                CreatedAt = p.CreatedAt
            });
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null)
            {
                throw new NotFoundException("No se puede eliminar: Paciente no encontrado.");
            }

            _repository.Delete(patient);
        }

        
    }
}
