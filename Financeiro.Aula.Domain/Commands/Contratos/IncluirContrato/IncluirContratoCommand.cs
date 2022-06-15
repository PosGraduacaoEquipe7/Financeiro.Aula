using Financeiro.Aula.Domain.Entities;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.IncluirContrato
{
    public class IncluirContratoCommand : IRequest<Contrato>
    {
        public DateTime DataEmissao { get; set; }

        public decimal ValorTotal { get; set; }

        public int NumeroParcelas { get; set; }

        public DateTime DataPrimeiroVencimento { get; set; }

        public long ClienteId { get; set; }
    }
}