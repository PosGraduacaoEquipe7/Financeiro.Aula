using Financeiro.Aula.Domain.DTOs;
using Financeiro.Aula.Domain.Interfaces.Queues;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.GerarBoletoParcela
{
    public class GerarBoletoParcelaCommandHandler : IRequestHandler<GerarBoletoParcelaCommand, (bool Sucesso, string Mensagem)>
    {
        private readonly IParcelaRepository _parcelaRepository;
        private readonly IBoletoQueue _boletoQueue;

        public GerarBoletoParcelaCommandHandler(IParcelaRepository parcelaRepository, IBoletoQueue boletoQueue)
        {
            _parcelaRepository = parcelaRepository;
            _boletoQueue = boletoQueue;
        }

        public async Task<(bool Sucesso, string Mensagem)> Handle(GerarBoletoParcelaCommand request, CancellationToken cancellationToken)
        {
            var parcela = await _parcelaRepository.ObterParcela(request.ParcelaId);

            if (parcela is null)
                return (false, "Parcela não localizada");

            if (parcela.Paga)
                return (false, "A parcela já está paga");

            if (parcela.TemBoleto && !request.ConfirmaSobrescrever)
                return (false, "A parcela já possui boleto gerado");

            await _boletoQueue.EnviarParcelaFilaGerarBoleto(new ParcelaGerarBoletoDto(parcela));

            return (true, string.Empty);
        }
    }
}