using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Infra.Context;
using Financeiro.Aula.Infra.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinanceiroDb>(options => options.UseInMemoryDatabase("Financeiro.Aula"));

builder.Services.AddScoped<IParcelaRepository, ParcelaRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IContratoRepository, ContratoRepository>();

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
