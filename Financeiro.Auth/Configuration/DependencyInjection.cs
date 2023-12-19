using Financeiro.Auth.Context;
using Financeiro.Auth.Entities;
using System.Data;

namespace Financeiro.Auth.Configuration
{
    public static class DependencyInjection
    {
        public static IHost EnsureDbCreated(this IHost app)
        {
            int tentativa = 0;

            var logger = app.Services.GetService<ILogger<Program>>();

            while (tentativa < 10)
            {
                tentativa++;

                logger?.LogInformation("Aguardando {tempo} segundos para criar AuthDb", tentativa);
                Thread.Sleep(tentativa * 1000);

                try
                {
                    var scope = app.Services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AuthDb>();
                    db.Database.EnsureCreated();

                    CriarDadosIniciais(db);

                    return app;
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "Não foi possível conectar ao AuthDb Context. Tentativa {tentativa}", tentativa);
                }
            }

            throw new DataException("Não foi possível conectar no AuthDb depois de 10 tentativas");
        }

        private static void CriarDadosIniciais(AuthDb db)
        {
            if (!db.Usuarios.Any())
            {
                db.Usuarios.Add(new Usuario(0, "Felipe", "felipejunges@yahoo.com.br", "felipe123", "Admin"));

                db.SaveChanges();
            }
        }
    }
}