using Application.DTOs.Common;
using Application.DTOs.Patient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetPatientsCreatedAfterAsync(DateTime date);
        Task<PagedResult<PatientDto>> GetAllAsync(int page, int pageSize, string? name, string? documentNumber);
        Task<PatientDto?> GetByIdAsync(int id);
        Task CreateAsync(CreatePatientDto dto);
        Task UpdateAsync(int id, UpdatePatientDto dto);
        Task DeleteAsync(int id);
    }
}
