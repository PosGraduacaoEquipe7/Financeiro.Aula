using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.AlterarBoletoParcela
{
    public class AlterarBoletoParcelaCommandHandler : IRequestHandler<AlterarBoletoParcelaCommand, (bool Sucesso, string Mensagem)>
    {
        private readonly IParcelaRepository _parcelaRepository;

        public AlterarBoletoParcelaCommandHandler(IParcelaRepository parcelaRepository)
        {
            _parcelaRepository = parcelaRepository;
        }

        public async Task<(bool Sucesso, string Mensagem)> Handle(AlterarBoletoParcelaCommand request, CancellationToken cancellationToken)
        {
            var parcela = await _parcelaRepository.ObterParcela(request.ParcelaId);

            if (parcela is null)
                return (false, "Parcela não localizada");

            if (parcela.Paga)
                return (false, "A parcela já está paga");

            if (parcela.TemBoleto)
                return (false, "A parcela já tem boleto vinculado");

            parcela.RegistrarBoleto(request.ChaveBoleto);

            await _parcelaRepository.AlterarParcela(parcela);

            return (true, string.Empty);
        }
    }
}