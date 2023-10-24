using FluentValidation;
using SGC.Domain.Entities.DTOs;

namespace SGC.Domain.Validators
{
    public class FormConnectionValidator : AbstractValidator<FormConnection>
    {
        public FormConnectionValidator()
        {
            RuleFor(fc => fc.Sgbd).NotEqual(0).WithMessage("O sgbd nao pode ser 0!");
            RuleFor(fc => fc.ConnString).NotEmpty().WithMessage("A connection string deve ser preenchida!");
        }
    }
}
