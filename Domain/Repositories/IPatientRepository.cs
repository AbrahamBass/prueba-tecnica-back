using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories
{
    public interface IPatientRepository
    {
        Task<List<Patients>> GetCreatedAfterAsync(DateTime date);
        Task AddAsync(Patients patient);
        Task<(List<Patients>, int TotalCount)> GetPagedAsync(int page, int pageSize, string? name, string? documentNumber);
        Task<Patients?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string documentType, string documentNumber);
        void Update(Patients patient);
        void Delete(Patients patient);
    }
}
