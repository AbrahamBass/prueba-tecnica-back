using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Patient
{
    public class PatientBaseDto
    {
        public string DocumentType { get; set; } 
        public string DocumentNumber { get; set; } 
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public DateOnly BirthDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
