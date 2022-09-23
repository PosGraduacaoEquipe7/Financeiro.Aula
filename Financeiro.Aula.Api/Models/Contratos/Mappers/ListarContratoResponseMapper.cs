using Financeiro.Aula.Api.Models.Contratos.Responses;
using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Api.Models.Contratos.Mappers
{
    public static class ListarContratoResponseMapper
    {
        public static IEnumerable<ListarContratoResponse> Map(IEnumerable<Contrato> contratos)
            => contratos.Select(Map);

        public static ListarContratoResponse Map(Contrato contrato)
        {
            return new ListarContratoResponse()
            {
                Id = contrato.Id,
                Curso = "Teste"
            };
        }
    }
}