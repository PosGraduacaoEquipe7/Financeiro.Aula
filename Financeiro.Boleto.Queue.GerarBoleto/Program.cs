using Financeiro.Boleto.Domain.Interfaces.ApiServices;
using Financeiro.Boleto.Domain.Interfaces.Repositories;
using Financeiro.Boleto.Domain.Interfaces.Services;
using Financeiro.Boleto.Domain.Services;
using Financeiro.Boleto.Infra.Context;
using Financeiro.Boleto.Infra.Repositories;
using Financeiro.Boleto.Infra.Services.ApiServices;
using Financeiro.Boleto.Queue.GerarBoleto;
using Financeiro.Boleto.Queue.GerarBoleto.Config;
using Financeiro.Boleto.Queue.GerarBoleto.Scopes;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddDbContext<BoletoDb>(options => options.UseSqlServer("Data Source=localhost;Initial Catalog=ApiBoleto;Persist Security Info=True;Encrypt=False;User ID=sa;Password=feherr")); // TODO: colocar no appsettings

        services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfig"));

        services
            .AddScoped<IBoletoService, BoletoService>()
            .AddScoped<IBoletoRepository, BoletoRepository>()
            .AddScoped<IParametroBoletoRepository, ParametroBoletoRepository>()
            .AddScoped<IScopedProcessingService, DefaultScopedProcessingService>();

        services.AddHttpClient<IGeradorBoletoApiService, BoletoCloudApiService>(client =>
        {
            var token = $"{configuration["ApiBoletoCloud:ApiKey"]}:token";

            client.BaseAddress = new Uri(configuration["ApiBoletoCloud:BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(token)));
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
