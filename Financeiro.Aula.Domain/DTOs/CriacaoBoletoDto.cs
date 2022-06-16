namespace Financeiro.Aula.Domain.DTOs
{
    public record CriacaoBoletoDto(bool Sucesso, string Numero, string Token, byte[] Pdf);
}