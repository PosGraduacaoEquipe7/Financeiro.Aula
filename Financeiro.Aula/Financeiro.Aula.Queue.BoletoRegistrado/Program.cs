using Financeiro.Aula.Domain.Configurations;
using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Services.DomainServices;
using Financeiro.Aula.Infra.Context;
using Financeiro.Aula.Infra.Repositories;
using Financeiro.Aula.Queue.BoletoRegistrado;
using Financeiro.Aula.Queue.BoletoRegistrado.Scopes;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddDbContext<FinanceiroDb>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfig"));

        services
            .AddScoped<IParcelaService, ParcelaService>()
            .AddScoped<IParcelaRepository, ParcelaRepository>()
            .AddScoped<IScopedProcessingService, DefaultScopedProcessingService>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
