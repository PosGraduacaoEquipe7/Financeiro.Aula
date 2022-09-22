using Financeiro.Aula.Api.Services;
using Financeiro.Aula.Domain.Extensions;
using Financeiro.Aula.Domain.Interfaces.ApiServices;
using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Interfaces.Services;
using Financeiro.Aula.Domain.Interfaces.Services.PDFs;
using Financeiro.Aula.Domain.Services.DomainServices;
using Financeiro.Aula.Domain.Services.PDFs;
using Financeiro.Aula.Infra.ApiServices.BoletoCloud;
using Financeiro.Aula.Infra.Repositories;
using System.Net.Http.Headers;

namespace Financeiro.Aula.Api.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection DeclareRepositorys(this IServiceCollection services)
        {
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IContratoRepository, ContratoRepository>();
            services.AddScoped<ICursoRepository, CursoMockRepository>();
            services.AddScoped<IEmpresaRepository, EmpresaMockRepository>();
            services.AddScoped<IParametroBoletoRepository, ParametroBoletoRepository>();
            services.AddScoped<IParcelaRepository, ParcelaRepository>();
            services.AddScoped<ITurmaRepository, TurmaMockRepository>();

            return services;
        }

        public static IServiceCollection DeclareServices(this IServiceCollection services)
        {
            services
                .AddScoped<IGeradorContratoPdfService, GeradorContratoPdfService>();

            return services;
        }

        public static IServiceCollection DeclareDomainServices(this IServiceCollection services)
        {
            services
                .AddScoped<IClienteService, ClienteService>()
                .AddScoped<IParcelaService, ParcelaService>();

            return services;
        }

        public static IServiceCollection DeclareApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IAuthService, AuthService>();

            services.AddHttpClient<IGeradorBoletoApiService, BoletoCloudApiService>(client =>
            {
                var token = $"{configuration["ApiBoletoCloud:ApiKey"]}:token";

                client.BaseAddress = new Uri(configuration["ApiBoletoCloud:BaseAddress"]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token.EncodeToBase64());
            });

            return services;
        }
    }
}