using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.ValueObjects;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Matriculas.GerarContratoDaMatricula
{
    public class GerarContratoDaMatriculaCommand : IRequest<Contrato?>
    {
        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Cpf { get; set; } = string.Empty;

        public string Identidade { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        public string Telefone { get; set; } = string.Empty;

        public Endereco Endereco { get; set; } = default!;

        public int CursoId { get; set; }

        public int NumeroParcelas { get; set; }
    }
}