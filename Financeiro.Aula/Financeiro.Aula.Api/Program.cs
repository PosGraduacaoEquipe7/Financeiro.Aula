using Financeiro.Aula.Api.Configuration;
using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Infra.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinanceiroDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .DeclareRepositorys()
    .DeclareServices()
    .DeclareDomainServices()
    .DeclareValidators()
    .DeclareQueues(builder.Configuration)
    .DeclareApiServices(builder.Configuration);

var domainAssembly = AppDomain.CurrentDomain.Load("Financeiro.Aula.Domain");
builder.Services.AddMediatR(domainAssembly);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.InstanceName = "ApiCache";
    opt.Configuration = "redis:6379";
});

//builder.Services.AddKeyCloakAuthentication();
builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddApiSwagger();

builder.Services.AddApiSeqLogging(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200", "http://localhost:5173", "http://127.0.0.1:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FinanceiroDb>();
    db.Database.EnsureCreated();

    if (!db.Cursos.Any())
    {
        var curso = new Curso(
            id: 0,
            descricao: "Nutrição",
            cargaHoraria: 100,
            valorBruto: 5000
        );
        db.Cursos.Add(curso);
        db.SaveChanges();

        var turma = new Turma(
            id: 0,
            numero: "1",
            horario: "SEG-QUA-SEX, 19h30-22h30",
            cursoId: curso.Id,
            dataInicio: new DateTime(2023, 3, 6),
            dataTermino: new DateTime(2023, 6, 30)
        );
        db.Turmas.Add(turma);

        db.SaveChanges();
    }
}

app.Run();
