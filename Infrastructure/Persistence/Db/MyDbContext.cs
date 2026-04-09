using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Db
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        
         public DbSet<Patients> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Patients>(entity =>
            {
                entity.HasKey(e => e.PatientId);

                entity.Property(e => e.PatientId)
                    .UseIdentityColumn();

                entity.Property(e => e.DocumentType)
                      .IsRequired()
                      .HasMaxLength(10); 

                entity.Property(e => e.DocumentNumber)
                      .IsRequired()
                      .HasMaxLength(20); 

                entity.HasIndex(e => e.DocumentNumber)
                      .IsUnique();

                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(80); 

                entity.Property(e => e.LastName)
                      .IsRequired()
                      .HasMaxLength(80); 

                entity.Property(e => e.BirthDate)
                      .IsRequired()
                      .HasColumnType("date"); 

                entity.Property(e => e.PhoneNumber)
                      .HasMaxLength(20)
                      .IsRequired(false); 

                entity.Property(e => e.Email)
                      .HasMaxLength(120) 
                      .IsRequired(false);

                entity.Property(e => e.CreatedAt)
                      .IsRequired()
                      .HasColumnType("datetime2") 
                      .HasDefaultValueSql("GETUTCDATE()"); 
            });
        }

        //protected override void Up(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.Sql(@"
        //        IF OBJECT_ID('GetPatientsCreatedAfter', 'P') IS NOT NULL
        //            DROP PROCEDURE GetPatientsCreatedAfter;
    
        //        EXEC('
        //            CREATE PROCEDURE GetPatientsCreatedAfter
        //                @Date DATETIME
        //            AS
        //            BEGIN
        //                SET NOCOUNT ON;

        //                SELECT * FROM Patients WHERE CreatedAt > @Date
        //            END
        //        ')
        //    ");
        //}

        //protected override void Down(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetPatientsCreatedAfter");
        //}
    }
}
