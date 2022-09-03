using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.ValueObjects;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Matriculas.GerarContratoDaMatricula
{
    public class GerarContratoDaMatriculaCommand : IRequest<Contrato?>
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string Cpf { get; set; }

        public string Identidade { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Telefone { get; set; }

        public Endereco Endereco { get; set; }

        public int CursoId { get; set; }

        public int NumeroParcelas { get; set; }
    }
}