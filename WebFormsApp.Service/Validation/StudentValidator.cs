using FluentValidation;
using System;
using WebFormsApp.Entity.Dtos;

namespace WebFormsApp.Service.Validation
{
    public class StudentValidator:AbstractValidator<StudentDto>
    {
        public StudentValidator()
        {
            RuleFor(x => x.UniqueId)
                .NotEmpty()
                .Length(11, 11);
            
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name cannot be empty")
                .Must(x => !string.IsNullOrWhiteSpace(x) && x == x.Trim())
                .Length(2, 50);
            
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name cannot be empty")
                .Must(x => !string.IsNullOrWhiteSpace(x) && x == x.Trim())
                .Length(2, 50);
            
            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.Now)
                .WithMessage("Birth Date can't be greater than Today's date");
            
            RuleFor(x => x.PlaceOfBirth)
                .NotEmpty()
                .WithMessage("PlaceOfBirth cannot be empty")
                .Length(2,50);
        }
    }
}
