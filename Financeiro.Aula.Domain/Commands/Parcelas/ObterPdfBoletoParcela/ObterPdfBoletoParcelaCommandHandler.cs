using Financeiro.Aula.Domain.Interfaces.ApiServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.ObterPdfBoletoParcela
{
    public class ObterPdfBoletoParcelaCommandHandler : IRequestHandler<ObterPdfBoletoParcelaCommand, (bool Sucesso, string Mensagem, string Pdf)>
    {
        private readonly IBoletoApiService _boletoApiService;
        private readonly IParcelaRepository _parcelaRepository;

        public ObterPdfBoletoParcelaCommandHandler(
            IBoletoApiService boletoApiService,
            IParcelaRepository parcelaRepository)
        {
            _boletoApiService = boletoApiService;
            _parcelaRepository = parcelaRepository;
        }

        public async Task<(bool Sucesso, string Mensagem, string Pdf)> Handle(ObterPdfBoletoParcelaCommand request, CancellationToken cancellationToken)
        {
            var parcela = await _parcelaRepository.ObterParcela(request.ParcelaId);

            if (parcela is null)
                return (false, "Parcela não localizada", string.Empty);

            if (!parcela.TemBoleto)
                return (false, "A parcela não possui boleto gerado", string.Empty);

            var boleto = await _boletoApiService.ObterPdfBoleto(parcela.ChaveBoleto!);

            if (boleto is null)
                return (false, "Erro ao obter os dados do boleto", string.Empty);

            return (true, string.Empty, boleto);
        }
    }
}