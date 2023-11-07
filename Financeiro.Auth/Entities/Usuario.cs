namespace Financeiro.Auth.Entities
{
    public class Usuario
    {
        public long Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; } // TODO: ValueObject
        public string Role { get; private set; }

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