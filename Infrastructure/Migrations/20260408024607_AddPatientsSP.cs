using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientsSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('GetPatientsCreatedAfter', 'P') IS NOT NULL
                    DROP PROCEDURE GetPatientsCreatedAfter;
        
                EXEC('
                    CREATE PROCEDURE GetPatientsCreatedAfter
                        @Date DATETIME
                    AS
                    BEGIN
                        SET NOCOUNT ON;

                        SELECT * FROM Patients WHERE CreatedAt > @Date
                    END
                ')
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetPatientsCreatedAfter");
        }
    }
}
