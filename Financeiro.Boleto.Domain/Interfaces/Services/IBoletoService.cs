using Financeiro.Boleto.Domain.DTOs;

namespace Financeiro.Boleto.Domain.Interfaces.Services
{
    public interface IBoletoService
    {
        Task RegistrarBoleto(BoletoGerarDto boletoDto);
    }
}