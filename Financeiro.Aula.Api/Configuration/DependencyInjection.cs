using Financeiro.Aula.Api.Services;
using Financeiro.Aula.Domain.Interfaces.ApiServices;
using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Domain.Interfaces.Queues;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Interfaces.Services;
using Financeiro.Aula.Domain.Interfaces.Services.PDFs;
using Financeiro.Aula.Domain.Services.DomainServices;
using Financeiro.Aula.Domain.Services.PDFs;
using Financeiro.Aula.Domain.Services.Queues;
using Financeiro.Aula.Infra.Repositories;
using Financeiro.Aula.Infra.Services.ApiServices;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Financeiro.Aula.Api.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection DeclareRepositorys(this IServiceCollection services)
        {
            services
                .AddScoped<IClienteRepository, ClienteRepository>()
                .AddScoped<IContratoRepository, ContratoRepository>()
                .AddScoped<ICursoRepository, CursoRepository>()
                .AddScoped<IEmpresaRepository, EmpresaMockRepository>()
                .AddScoped<IParcelaRepository, ParcelaRepository>()
                .AddScoped<ITurmaRepository, TurmaRepository>();

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

        public static IServiceCollection DeclareQueues(this IServiceCollection services)
        {
            services
                .AddScoped<IBoletoQueue, BoletoQueue>();

            return services;
        }

        public static IServiceCollection DeclareApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IAuthService, AuthService>();

            services.AddHttpClient<IBoletoApiService, BoletoApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration["ApiBoleto:BaseAddress"]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", context.Header[""]);
            });

            return services;
        }
    }
}