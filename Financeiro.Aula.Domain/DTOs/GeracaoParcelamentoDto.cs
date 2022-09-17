using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.DTOs
{
    public record GeracaoParcelamentoDto(bool Sucesso, string MensagemErro, List<Parcela>? Parcelas);
}