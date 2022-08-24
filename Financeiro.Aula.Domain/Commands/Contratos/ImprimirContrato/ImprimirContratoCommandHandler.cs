using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Interfaces.Services.PDFs;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Contratos.ImprimirContrato
{
    public class ImprimirContratoCommandHandler : IRequestHandler<ImprimirContratoCommand, (bool Sucesso, string Mensagem, string Contrato)>
    {
        private readonly IGeradorContratoPdfService _geradorContratoPdfService;
        private readonly IContratoRepository _contratoRepository;
        private readonly ITurmaRepository _turmaRepository;

        public ImprimirContratoCommandHandler(IGeradorContratoPdfService geradorContratoPdfService, IContratoRepository contratoRepository, ITurmaRepository turmaRepository)
        {
            _geradorContratoPdfService = geradorContratoPdfService;
            _contratoRepository = contratoRepository;
            _turmaRepository = turmaRepository;
        }

        public async Task<(bool Sucesso, string Mensagem, string Contrato)> Handle(ImprimirContratoCommand request, CancellationToken cancellationToken)
        {
            var contrato = await _contratoRepository.ObterContratoComParcelasECliente(request.Id);

            if (contrato is null)
                return (false, "Contrato não encontrado", string.Empty);

            if (contrato.Cancelado)
                return (false, "O contrato está cancelado", string.Empty);

            // TODO: validar se o contrato é do aluno logado

            var turma = await _turmaRepository.ObterTurmaPadrao();
            if (turma is not null)
                contrato.AgregarTurma(turma);

            try
            {
                var pdfStream = await _geradorContratoPdfService.GerarContratoMatricula(contrato);
                if (pdfStream is null)
                    return (false, "Não foi possível gerar o PDF do documento", string.Empty);

                string pdf = Convert.ToBase64String(pdfStream);

                return (true, string.Empty, pdf);
            }
            catch (Exception ex)
            {
                return (false, $"Ocorreu um erro ao gerar o PDF do documento: {ex.Message}", string.Empty);
            }
        }
    }
}