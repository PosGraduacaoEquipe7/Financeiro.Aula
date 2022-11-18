using Financeiro.Boleto.Domain.Entities;
using Financeiro.Boleto.Domain.Interfaces.Repositories;
using Financeiro.Boleto.Domain.ValueObjects;
using Financeiro.Boleto.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Boleto.Infra.Repositories
{
    public class ParametroBoletoRepository : IParametroBoletoRepository
    {
        private const long PARAMETRO_BOLETO_PADRAO = 1;

        private readonly BoletoDb _context;

        public ParametroBoletoRepository(BoletoDb context)
        {
            _context = context;
        }

        public async Task<ParametroBoleto?> ObterParametrosBoleto()
        {
            await TEMP();

            return await _context.ParametrosBoleto
                            .Where(p => p.Id == PARAMETRO_BOLETO_PADRAO)
                            .FirstOrDefaultAsync();
        }

        private async Task TEMP()
        {
            if (await _context.ParametrosBoleto.AnyAsync())
                return;

            await _context.ParametrosBoleto.AddAsync(
                new ParametroBoleto(
                        id: 1,
                        descricao: "Boleto Bradesco",
                        banco: "237",
                        agencia: "1234-5",
                        numeroConta: "123456-0",
                        carteira: "12",
                        numeroBoletoAtual: 0,
                        nomeBeneficiario: "Financeiro Aula Solutions",
                        cnpjBeneficiario: "09.934.582/0001-58",
                        enderecoBeneficiario: new Endereco(
                            cep: "93000-000",
                            logradouro: "Rua das Empresas",
                            numero: "112",
                            complemento: "",
                            bairro: "Centro",
                            municipio: "Porto Alegre",
                            uf: "RS")
                        ));

            await _context.SaveChangesAsync();
        }
    }
}