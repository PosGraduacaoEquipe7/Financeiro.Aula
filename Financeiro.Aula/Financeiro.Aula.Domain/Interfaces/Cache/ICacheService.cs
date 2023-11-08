namespace Financeiro.Aula.Domain.Interfaces.Cache
{
    public interface ICacheService
    {
        Task Set<T>(string key, T value);
        Task<T?> Get<T>(string key);
    }
}