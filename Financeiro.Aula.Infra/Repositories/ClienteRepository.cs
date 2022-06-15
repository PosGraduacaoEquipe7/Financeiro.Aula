using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Aula.Infra.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly FinanceiroDb _context;

        public ClienteRepository(FinanceiroDb context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> ListarClientes()
        {
            return await _context.Clientes.OrderBy(c => c.Nome).ToListAsync();
        }

        public async Task<Cliente?> IncluirCliente(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();

            return cliente;
        }
    }
}