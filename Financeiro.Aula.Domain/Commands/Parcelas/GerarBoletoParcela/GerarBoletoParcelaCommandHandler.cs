﻿using Financeiro.Aula.Domain.Interfaces.ApiServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.GerarBoletoParcela
{
    public class GerarBoletoParcelaCommandHandler : IRequestHandler<GerarBoletoParcelaCommand, (bool Sucesso, string Mensagem, string Pdf)>
    {
        private readonly IGeradorBoletoApiService _geradorBoletoApiService;
        private readonly IParcelaRepository _parcelaRepository;

        public GerarBoletoParcelaCommandHandler(IGeradorBoletoApiService geradorBoletoApiService, IParcelaRepository parcelaRepository)
        {
            _geradorBoletoApiService = geradorBoletoApiService;
            _parcelaRepository = parcelaRepository;
        }

        public async Task<(bool Sucesso, string Mensagem, string Pdf)> Handle(GerarBoletoParcelaCommand request, CancellationToken cancellationToken)
        {
            var parcela = await _parcelaRepository.ObterParcela(request.ParcelaId);

            if (parcela is null)
                return (false, "Parcela não localizada", string.Empty);

            if (parcela.Paga)
                return (false, "A parcela já está paga", string.Empty);

            if (parcela.TemBoleto && !request.ConfirmaSobrescrever)
                return (false, "A parcela já possui boleto gerado", string.Empty);

            var resultado = await _geradorBoletoApiService.GerarBoleto(parcela);

            if (!resultado.Sucesso)
                return (false, $"Erro ao gerar o boleto: {resultado.MensagemErro}", string.Empty);

            parcela.RegistrarBoleto(resultado.Numero, resultado.Token);
            await _parcelaRepository.AlterarParcela(parcela);

            string pdf = Convert.ToBase64String(resultado.Pdf);

            return (true, string.Empty, pdf);
        }
    }
}