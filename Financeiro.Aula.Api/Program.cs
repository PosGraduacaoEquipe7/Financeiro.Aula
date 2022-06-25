using Financeiro.Aula.Api.Configuration;
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
}

app.Run();
