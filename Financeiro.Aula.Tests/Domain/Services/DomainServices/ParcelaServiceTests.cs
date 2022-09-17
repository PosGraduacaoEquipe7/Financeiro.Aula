using Financeiro.Aula.Domain.Services.DomainServices;

namespace Financeiro.Aula.Tests.Domain.Services.DomainServices
{
    public class ParcelaServiceTests
    {
        private readonly ParcelaService _parcelaService;

        public ParcelaServiceTests()
        {
            _parcelaService = new ParcelaService();
        }

        // Se o número de parcelas informado for zero, não pode ocorrer erro por divisão por zero
        [Fact]
        public void NumeroParcelasZeradoNaoPodeCausarException()
        {
            // Dado que não pode ocorrer exception
            var numeroParcelas = 0;

            // Quando o número de parcelas for zero
            var resultado = _parcelaService.GerarParcelas(1000, numeroParcelas, DateTime.Now.Date, 0);

            // Então
            Assert.Empty(resultado);
        }

        // Se o número de parcelas informado for zero, não pode ocorrer erro por divisão por zero
        [Theory]
        [InlineData(1000, 10, 100)]
        [InlineData(2000, 10, 200)]
        [InlineData(2000, 5, 400)]
        public void ParcelamentoDeValorExatoDeveSerCalculadoCorretamente(double valorTotal, int numeroParcelas, double valorParcelas)
        {
            // Dado que o cálculo do parcelamento deve estar correto

            // Quando o valor total for 1.000 em 10 parcelas
            var resultado = _parcelaService.GerarParcelas(valorTotal, numeroParcelas, DateTime.Now.Date, 0);

            // Então
            Assert.NotEmpty(resultado);
            Assert.True(resultado.Count == numeroParcelas);
            Assert.All(resultado, r => Assert.Equal(valorParcelas, r.Valor));
        }

        // Se o número de parcelas informado for zero, não pode ocorrer erro por divisão por zero
        [Theory]
        [InlineData(1400, 9, 155.52, 155.56)]
        [InlineData(1400, 11, 127.3, 127.27)]
        [InlineData(1700, 13, 130.76, 130.77)]
        public void ParcelamentoDeValorNaoExatoDeveSerCalculadoCorretamente(double valorTotal, int numeroParcelas, double valorPrimeira, double valorParcelas)
        {
            // Dado que o cálculo do parcelamento deve estar correto

            // Quando o valor total for 1.000 em 10 parcelas
            var resultado = _parcelaService.GerarParcelas(valorTotal, numeroParcelas, DateTime.Now.Date, 0);

            // Então
            Assert.NotEmpty(resultado);
            Assert.True(resultado.Count == numeroParcelas);

            Assert.Equal(valorPrimeira, resultado.First().Valor);
            Assert.All(resultado.Skip(1), r => Assert.Equal(valorParcelas, r.Valor));
        }
    }
}