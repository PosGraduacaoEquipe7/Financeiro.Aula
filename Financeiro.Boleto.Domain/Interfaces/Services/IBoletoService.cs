﻿using Financeiro.Boleto.Domain.DTOs;

namespace Financeiro.Boleto.Domain.Interfaces.Services
{
    public interface IBoletoService
    {
        Task<string?> ObterPdfBoleto(Guid id);
        Task<bool> RegistrarBoleto(BoletoGerarDto boletoDto);
    }
}