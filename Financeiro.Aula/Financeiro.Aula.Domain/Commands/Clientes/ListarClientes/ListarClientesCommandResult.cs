namespace Financeiro.Aula.Domain.Commands.Clientes.ListarClientes
{
    public class ListarClientesCommandResult
    {
        public long Id { get; private set; }

        public string Nome { get; private set; }

        public string Email { get; private set; }

        public ListarClientesCommandResult(long id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }
    }
}