using Financeiro.Aula.Domain.Extensions;
using Financeiro.Aula.Domain.Interfaces.ApiServices;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Infra.ApiServices.BoletoCloud;
using Financeiro.Aula.Infra.Context;
using Financeiro.Aula.Infra.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<FinanceiroDb>(options => options.UseInMemoryDatabase("Financeiro.Aula"));
builder.Services.AddDbContext<FinanceiroDb>(options => options.UseSqlite("DataSource=Db/Financeiro.db"));

builder.Services.AddScoped<IParcelaRepository, ParcelaRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IContratoRepository, ContratoRepository>();

builder.Services.AddHttpClient<IGeradorBoletoApiService, IBoletoCloudApiService>(client =>
{
    var token = $"{builder.Configuration["ApiBoletoCloud:ApiKey"]}:token";

    client.BaseAddress = new Uri(builder.Configuration["ApiBoletoCloud:BaseAddress"]);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token.EncodeToBase64());
});

var domainAssembly = AppDomain.CurrentDomain.Load("Financeiro.Aula.Domain");
builder.Services.AddMediatR(domainAssembly);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
