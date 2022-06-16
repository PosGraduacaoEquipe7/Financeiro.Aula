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

        public async Task<Cliente?> ObterCliente(long id)
        {
            return await _context.Clientes.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Cliente>> ListarClientes(string? nome)
        {
            _context.Database.EnsureCreated();

            return await _context.Clientes
                            .Where(c => 
                                string.IsNullOrEmpty(nome) || c.Nome.Contains(nome))
                            .OrderBy(c => c.Nome)
                            .ToListAsync();
        }

        public async Task<Cliente?> IncluirCliente(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();

            return cliente;
        }

        public async Task AtualizarCliente(Cliente cliente)
        {
            await Task.Run(() => _context.Clientes.Update(cliente));
            await _context.SaveChangesAsync();
        }
    }
}