using FluentValidation;

namespace Financeiro.Aula.Domain.Commands.Clientes.IncluirCliente
{
    public class IncluirClienteCommandValidator : AbstractValidator<IncluirClienteCommand>
    {
        public IncluirClienteCommandValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("O nome é obrigatório");
        }
    }
}