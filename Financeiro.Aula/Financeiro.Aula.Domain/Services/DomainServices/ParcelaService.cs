using Financeiro.Aula.Domain.DTOs;
using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;

namespace Financeiro.Aula.Domain.Services.DomainServices
{
    public class ParcelaService : IParcelaService
    {
        private const double VALOR_MINIMO_PARCELA = 100;
        private const int NUMERO_MAXIMO_PARCELAS = 24;
        private readonly IParcelaRepository _parcelaRepository;

        public ParcelaService(IParcelaRepository parcelaRepository)
        {
            _parcelaRepository = parcelaRepository;
        }

        public GeracaoParcelamentoDto GerarParcelas(double valorTotal, int numeroParcelas, DateTime primeiroVencimento, long contratoId)
        {
            if (numeroParcelas <= 0)
                return new(false, "O número de parcelas deve ser maior do que zero", null);

            if (numeroParcelas > NUMERO_MAXIMO_PARCELAS)
                return new(false, $"O número de parcelas máximo é {NUMERO_MAXIMO_PARCELAS}", null);

            double valorParcelas = CalcularValorParcela(valorTotal, numeroParcelas);
            var valorPrimeiraParcela = CalcularValorPrimeiraParcela(valorTotal, numeroParcelas, valorParcelas);

            if (valorParcelas < VALOR_MINIMO_PARCELA || valorPrimeiraParcela < VALOR_MINIMO_PARCELA)
                return new(false, $"O valor mínimo das parcelas deve ser de {VALOR_MINIMO_PARCELA:c2}", null);
            
            return GerarParcelasCorreto(numeroParcelas, primeiroVencimento, contratoId, valorParcelas, valorPrimeiraParcela);
        }

        public async Task<(bool Sucesso, string Mensagem)> AlterarChaveBoletoParcela(Guid tokenBoleto, string chaveBoleto)
        {
            var parcela = await _parcelaRepository.ObterParcelaPeloTokenBoleto(tokenBoleto);

            if (parcela is null)
                return (false, "Parcela não localizada");

            if (parcela.Paga)
                return (false, "A parcela já está paga");

            if (parcela.TemBoleto)
                return (false, "A parcela já tem boleto vinculado");

            parcela.RegistrarBoleto(chaveBoleto);

            await _parcelaRepository.AlterarParcela(parcela);

            return (true, string.Empty);
        }

        private static GeracaoParcelamentoDto GerarParcelasCorreto(int numeroParcelas, DateTime primeiroVencimento, long contratoId, double valorParcelas, double valorPrimeiraParcela)
        {
            var parcelas = new List<Parcela>();

            for (int i = 1; i <= numeroParcelas; i++)
            {
                var parcela = new Parcela(
                    id: 0,
                    sequencial: i,
                    valor: i == 1 ? valorPrimeiraParcela : valorParcelas,
                    dataVencimento: primeiroVencimento.AddMonths(i - 1),
                    contratoId: contratoId);

                parcelas.Add(parcela);
            }

            return new(true, string.Empty, parcelas);
        }

        private double CalcularValorParcela(double valorTotal, int numeroParcelas) => Math.Round(valorTotal / numeroParcelas, 2);

        private double CalcularValorPrimeiraParcela(double valorTotal, int numeroParcelas, double valorParcelas) => Math.Round(valorTotal - valorParcelas * (numeroParcelas - 1), 2);
    }
}