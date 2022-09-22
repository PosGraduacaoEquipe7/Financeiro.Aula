using Financeiro.Aula.Domain.ValueObjects;

namespace Financeiro.Aula.Domain.DTOs
{
    public record ClienteAtualizacaoDto(
        string UsuarioId,
        string Nome,
        string Email,
        string Cpf,
        string Identidade,
        DateTime DataNascimento,
        string Telefone,
        Endereco Endereco
    );
}