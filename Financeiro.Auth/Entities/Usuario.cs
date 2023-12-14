using Financeiro.Auth.ValueObjects;

namespace Financeiro.Auth.Entities
{
    public class Usuario
    {
        public long Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public Senha Senha { get; private set; }
        public string Role { get; private set; }
        public ICollection<Acesso> Acessos { get; private set; } = new HashSet<Acesso>();

        private Usuario()
        {
            Nome = string.Empty;
            Email = string.Empty;
            Senha = string.Empty;
            Role = string.Empty;
        }

        public Usuario(long id, string nome, string email, string senha, string role)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Senha = senha;
            Role = role;
        }
    }
}