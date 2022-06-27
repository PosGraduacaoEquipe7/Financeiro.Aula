using Financeiro.Aula.Api.Models.Contratos.Responses;
using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Api.Models.Contratos.Mappers
{
    public static class IncluirContratoResponseMapper
    {
        public static IncluirContratoResponse Map(Contrato contrato)
        {
            return new IncluirContratoResponse()
            {
                Id = contrato.Id,
                DataEmissao = contrato.DataEmissao,
                ValorTotal = contrato.ValorTotal,
                ClienteId = contrato.ClienteId
            };
        }
    }
}