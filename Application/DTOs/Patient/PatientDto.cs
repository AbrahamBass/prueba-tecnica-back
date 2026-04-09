using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Patient
{
    public class PatientDto : PatientBaseDto
    {
        public int PatientId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
