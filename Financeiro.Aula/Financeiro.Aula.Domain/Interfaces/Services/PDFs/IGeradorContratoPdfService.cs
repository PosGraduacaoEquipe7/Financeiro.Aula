using Financeiro.Aula.Domain.Entities;

namespace Financeiro.Aula.Domain.Interfaces.Services.PDFs
{
    public interface IGeradorContratoPdfService
    {
        Task<byte[]?> GerarContratoMatricula(Contrato contrato);
    }
}