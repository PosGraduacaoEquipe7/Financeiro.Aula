using Financeiro.Aula.Domain.Interfaces.Queues;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Moq;

namespace Financeiro.Aula.Tests.Domain.Commands.Parcelas.GerarBoletoParcela
{
    public class GerarBoletoParcelaCommandTests
    {
        //private readonly Mock<IGeradorBoletoApiService> _geradorBoletoApiService;
        private readonly Mock<IParcelaRepository> _parcelaRepository;
        private readonly Mock<IBoletoQueue> _boletoQueue;

        //private readonly GerarBoletoParcelaCommandHandler _commandHandler;

        public GerarBoletoParcelaCommandTests()
        {
            //_geradorBoletoApiService = new Mock<IGeradorBoletoApiService>();
            _parcelaRepository = new Mock<IParcelaRepository>();
            _boletoQueue = new Mock<IBoletoQueue>();

            //_commandHandler = new GerarBoletoParcelaCommandHandler(
            //    _geradorBoletoApiService.Object,
            //    _parcelaRepository.Object,
            //    _boletoQueue.Object
            //);
        }

        //[Fact]
        //public async void ParcelaNaoLocalizadaDeveRetornarInsucesso()
        //{
        //    // Arrange
        //    _parcelaRepository.Setup(s => s.ObterParcela(It.IsAny<long>())).ReturnsAsync((Parcela?)null);

        //    // Act
        //    var result = await _commandHandler.Handle(new GerarBoletoParcelaCommand(1, false), new CancellationToken());

        //    // Assert
        //    Assert.False(result.Sucesso);
        //    Assert.Equal("Parcela não localizada", result.Mensagem);
        //}
    }
}