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
            Assert.False(resultado.Sucesso);
            Assert.Null(resultado.Parcelas);
        }

        [Theory]
        [InlineData(1000, 10, 100)]
        [InlineData(2000, 10, 200)]
        [InlineData(2000, 5, 400)]
        public void ParcelamentoDeValorExatoDeveSerCalculadoCorretamente(double valorTotal, int numeroParcelas, double valorParcelas)
        {
            // Dado que o cálculo do parcelamento deve estar correto

            // Quando o valor total for de acordo com o recebido
            var resultado = _parcelaService.GerarParcelas(valorTotal, numeroParcelas, DateTime.Now.Date, 0);

            // Então
            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Parcelas);
            Assert.NotEmpty(resultado.Parcelas);
            Assert.True(resultado.Parcelas!.Count == numeroParcelas);

            Assert.All(resultado.Parcelas!, r => Assert.Equal(valorParcelas, r.Valor));
        }

        [Theory]
        [InlineData(1400, 9, 155.52, 155.56)]
        [InlineData(1400, 11, 127.3, 127.27)]
        [InlineData(1700, 13, 130.76, 130.77)]
        public void ParcelamentoDeValorNaoExatoDeveSerCalculadoCorretamente(double valorTotal, int numeroParcelas, double valorPrimeira, double valorParcelas)
        {
            // Dado que o cálculo do parcelamento deve estar correto

            // Quando o valor total for de acordo com o recebido
            var resultado = _parcelaService.GerarParcelas(valorTotal, numeroParcelas, DateTime.Now.Date, 0);

            // Então
            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Parcelas);
            Assert.NotEmpty(resultado.Parcelas);
            Assert.True(resultado.Parcelas!.Count == numeroParcelas);

            Assert.Equal(valorPrimeira, resultado.Parcelas.First().Valor);
            Assert.All(resultado.Parcelas.Skip(1), r => Assert.Equal(valorParcelas, r.Valor));
        }

        [Theory]
        [InlineData(2400, 24, true)]
        [InlineData(2500, 25, false)]
        [InlineData(1000, -1, false)]
        [InlineData(1000, 0, false)]
        [InlineData(1000, 1, true)]
        public void DeveSerValidadoValorLimiteDoNumeroParcelasMaximo(double valorTotal, int numeroParcelas, bool devePassarTeste)
        {
            // Dado que o cálculo do parcelamento deve estar correto

            // Quando o valor total for de acordo com o recebido
            var resultado = _parcelaService.GerarParcelas(valorTotal, numeroParcelas, DateTime.Now.Date, 0);

            // Então
            if (devePassarTeste)
            {
                Assert.True(resultado.Sucesso);
                Assert.NotNull(resultado.Parcelas);
                Assert.NotEmpty(resultado.Parcelas);
                Assert.True(resultado.Parcelas!.Count == numeroParcelas);
            }
            else
            {
                Assert.False(resultado.Sucesso);
                Assert.Null(resultado.Parcelas);
            }
        }
    }
}