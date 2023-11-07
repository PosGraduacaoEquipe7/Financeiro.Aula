using Financeiro.Aula.Domain.ValueObjects;

namespace Financeiro.Aula.Domain.Interfaces.Services.CEPs
{
    public interface ICEPService
    {
        Task<Endereco?> BuscarCEP(string cep);
    }
}