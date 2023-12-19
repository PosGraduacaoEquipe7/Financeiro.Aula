using Financeiro.Boleto.Infra.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;

namespace Financeiro.Boleto.Api.Configuration
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

                logger?.LogInformation("Aguardando {tempo} segundos para criar BoletoDb", tentativa);
                Thread.Sleep(tentativa * 1000);

                try
                {
                    using var scope = app.Services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<BoletoDb>();
                    db.Database.EnsureCreated();

                    return app;
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "Não foi possível conectar ao BoletoDb Context. Tentativa {tentativa}", tentativa);
                }
            }

            throw new DataException("Não foi possível conectar no BoletoDb depois de 10 tentativas");
        }

        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var keySecret = configuration.GetSection("Auth:KeySecret")?.Value ?? throw new NullReferenceException("'Auth:KeySecret' deve ser configurado no appsettings.json");
            var secret = Encoding.ASCII.GetBytes(keySecret);

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static IServiceCollection AddKeyCloakAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.Authority = "http://keycloak:8085/realms/myrealm"; // TODO: appsettings
                o.Audience = "account";
                o.RequireHttpsMetadata = false;

                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        //if (Environment.IsDevelopment())
                        //{
                        //return c.Response.WriteAsync(c.Exception.ToString());
                        Console.WriteLine(c.Exception.ToString());
                        return Task.FromResult(string.Empty);
                        //}
                        //return c.Response.WriteAsync("An error occured processing your authentication.");
                    }
                };
            });

            return services;
        }
    }
}