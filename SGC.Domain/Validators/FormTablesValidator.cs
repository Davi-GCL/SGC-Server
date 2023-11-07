using FluentValidation;
using SGC.Domain.Entities.DTOs;

namespace SGC.Domain.Validators
{
    public class FormTablesValidator : AbstractValidator<FormTables>
    {
        public FormTablesValidator() 
        {
            RuleFor(ft => ft.Sgbd).NotEqual(0).WithMessage("O sgbd nao pode ser 0!");
            RuleFor(ft => ft.ConnString).NotEmpty().WithMessage("A connection string não pode ser vazia!");
            RuleFor(ft => ft.Namespace).NotEmpty().WithMessage("O namespace não pode ser vazio!");
            RuleFor(ft => ft.SelectedTablesNames).NotEmpty().WithMessage("A lista de tabelas selecionadas não pode ser vazia!");
        }
    }
}
