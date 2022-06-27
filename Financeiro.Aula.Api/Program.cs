using Financeiro.Aula.Api.Configuration;
using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.ValueObjects;
using Financeiro.Aula.Infra.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinanceiroDb>(options => options.UseSqlite("DataSource=Db/Financeiro.db"));

builder.Services.DeclareRepositorys();
builder.Services.DeclareApiServices(builder.Configuration);

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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FinanceiroDb>();
    db.Database.EnsureCreated();

    if (!db.ParametrosBoleto.Any())
    {
        db.ParametrosBoleto.Add(
                   new ParametroBoleto(
                       id: 1,
                       descricao: "Boleto Bradesco",
                       banco: "237",
                       agencia: "1234-5",
                       numeroConta: "123456-0",
                       carteira: "12",
                       numeroBoletoAtual: 0,
                       nomeBeneficiario: "Financeiro Aula Solutions",
                       cnpjBeneficiario: "09.934.582/0001-58",
                       enderecoBeneficiario: new Endereco(
                               cep: "93000-000",
                               logradouro: "Rua das Empresas",
                               numero: "112",
                               complemento: "",
                               bairro: "Centro",
                               municipio: "Porto Alegre",
                               uf: "RS")
                           ));
                           
        db.SaveChanges();
    }
}

app.Run();
