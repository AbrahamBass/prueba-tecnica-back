using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Db;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly MyDbContext _context;
        public PatientRepository(MyDbContext context) 
        { 
            _context = context;
        }

        public async Task AddAsync(Patients patient)
        {
            try
            {
                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (IsUniqueConstraintViolation(ex))
                {
                    throw new Exception("El paciente ya existe.");
                }

                throw;
            }
        }

        public async Task<bool> ExistsAsync(string documentType, string documentNumber)
        {
            return await _context.Patients.AnyAsync(p => p.DocumentType == documentType && p.DocumentNumber == documentNumber);
        }

        public void Update(Patients patient)
        {
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }

        public void Delete(Patients patient)
        {
            _context.Patients.Remove(patient);
            _context.SaveChanges();
        }

        public async Task<Patients?> GetByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task<(List<Patients>, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? name,
            string? documentNumber
        )
        {
            var query = _context.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => (p.FirstName + " " + p.LastName).Contains(name));
            }

            if (!string.IsNullOrEmpty(documentNumber))
            {
                query = query.Where(p => p.DocumentNumber.Contains(documentNumber));
            }

            var totalCount = await query.CountAsync();

            var patients = await query
               .OrderBy(p => p.PatientId)
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            return (patients, totalCount);
        }

        public async Task<List<Patients>> GetCreatedAfterAsync(DateTime date)
        {
            return await _context.Patients
                .FromSqlRaw("EXEC GetPatientsCreatedAfter @Date",
                    new SqlParameter("@Date", date))
                .ToListAsync();
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            return ex.InnerException is SqlException sqlEx &&
                   (sqlEx.Number == 2601 || sqlEx.Number == 2627);
        }
    }
}
