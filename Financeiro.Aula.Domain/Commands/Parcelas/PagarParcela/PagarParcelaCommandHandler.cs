using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Parcelas.PagarParcela
{
    public class PagarParcelaCommandHandler : IRequestHandler<PagarParcelaCommand, (bool Sucesso, string Mensagem)>
    {
        private readonly IParcelaRepository _parcelaRepository;

        public PagarParcelaCommandHandler(IParcelaRepository parcelaRepository)
        {
            _parcelaRepository = parcelaRepository;
        }

        public async Task<(bool Sucesso, string Mensagem)> Handle(PagarParcelaCommand request, CancellationToken cancellationToken)
        {
            var parcela = await _parcelaRepository.ObterParcela(request.Id);

            if (parcela is null)
                return (false, "Parcela não localizada");

            if (parcela.Paga)
                return (false, "A parcela já está paga");

            parcela.Pagar();

            await _parcelaRepository.AlterarParcela(parcela);

            return (true, String.Empty);
        }
    }
}