using System;
using System.Collections.Generic;
using System.Text;
using Application.DTOs.Patient;
using FluentValidation;

namespace Application.Validators
{
    

    public class PatientDtoValidator : AbstractValidator<PatientBaseDto>
    {
        public PatientDtoValidator()
        {
            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("El documento es obligatorio.")
                .Matches(@"^[0-9]+$").WithMessage("El número de documento debe contener solo dígitos.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^[0-9]*$").WithMessage("El teléfono debe contener solo dígitos.");

            RuleFor(x => x).Custom((dto, context) =>
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var age = today.Year - dto.BirthDate.Year;
                if (dto.BirthDate > today.AddYears(-age)) age--;

                var docType = dto.DocumentType?.ToUpper();

                if (docType == "RC" && age > 6)
                    context.AddFailure("DocumentType", "El Registro Civil solo es válido hasta los 6 años.");

                else if (docType == "TI" && (age < 7 || age > 17))
                    context.AddFailure("DocumentType", "La Tarjeta de Identidad es para personas entre 7 y 17 años.");

                else if (docType == "CC" && age < 18)
                    context.AddFailure("DocumentType", "La Cédula de Ciudadanía es solo para mayores de 18 años.");
            });
        }
    }
}
