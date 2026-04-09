using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    // Excepción para conflictos de duplicidad (400 Bad Request o 409 Conflict)
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }

    // Excepción para elementos no encontrados (404 Not Found)
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
