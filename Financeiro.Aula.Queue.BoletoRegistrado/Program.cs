using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Services.DomainServices;
using Financeiro.Aula.Infra.Context;
using Financeiro.Aula.Infra.Repositories;
using Financeiro.Aula.Queue.BoletoRegistrado;
using Financeiro.Aula.Queue.BoletoRegistrado.Config;
using Financeiro.Aula.Queue.BoletoRegistrado.Scopes;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddDbContext<FinanceiroDb>(options => options.UseSqlServer("Data Source=localhost;Initial Catalog=ApiBoleto;Persist Security Info=True;Encrypt=False;User ID=sa;Password=feherr")); // TODO: colocar no appsettings

        services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfig"));

        services
            .AddScoped<IParcelaService, ParcelaService>()
            .AddScoped<IParcelaRepository, ParcelaRepository>()
            .AddScoped<IScopedProcessingService, DefaultScopedProcessingService>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
