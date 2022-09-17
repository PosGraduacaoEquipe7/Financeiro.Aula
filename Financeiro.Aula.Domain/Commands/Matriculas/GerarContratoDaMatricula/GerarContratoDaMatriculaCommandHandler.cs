using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using MediatR;

namespace Financeiro.Aula.Domain.Commands.Matriculas.GerarContratoDaMatricula
{
    public class GerarContratoDaMatriculaCommandHandler : IRequestHandler<GerarContratoDaMatriculaCommand, Contrato?>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IContratoRepository _contratoRepository;
        private readonly IParcelaRepository _parcelaRepository;
        private readonly ICursoRepository _cursoRepository;

        public GerarContratoDaMatriculaCommandHandler(
            IClienteRepository clienteRepository,
            IContratoRepository contratoRepository,
            IParcelaRepository parcelaRepository,
            ICursoRepository cursoRepository)
        {
            _clienteRepository = clienteRepository;
            _contratoRepository = contratoRepository;
            _parcelaRepository = parcelaRepository;
            _cursoRepository = cursoRepository;
        }

        public async Task<Contrato?> Handle(GerarContratoDaMatriculaCommand request, CancellationToken cancellationToken)
        {
            if (await _clienteRepository.VerificarCpfExiste(request.Cpf))
            {
                // TODO: cpf já existente                
                return default;
            }

            // TODO: salvar o e-mail e senha

            var cliente = new Cliente(
                id: 0,
                nome: request.Nome,
                cpf: request.Cpf,
                identidade: request.Identidade,
                dataNascimento: request.DataNascimento,
                telefone: request.Telefone,
                endereco: request.Endereco);

            await _clienteRepository.IncluirCliente(cliente);

            var curso = await _cursoRepository.ObterCursoPadrao();

            if (curso is null)
            {
                // TODO: curso inválido
                return default;
            }

            // TODO: setar a turma para o contrato

            var contrato = new Contrato(
                id: 0,
                dataEmissao: DateTime.Now,
                valorTotal: curso.ValorBruto,
                clienteId: cliente.Id);

            await _contratoRepository.IncluirContrato(contrato);

            // TODO: criar um Service que gera um array de parcelas
            for (int i = 1; i <= request.NumeroParcelas; i++)
            {
                var parcela = new Parcela(
                    id: 0,
                    sequencial: i,
                    valor: curso.ValorBruto / request.NumeroParcelas,
                    dataVencimento: DateTime.Now.Date.AddMonths(i),
                    contratoId: contrato.Id);

                await _parcelaRepository.IncluirParcela(parcela);
            }

            return contrato;
        }
    }
}