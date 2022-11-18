using Financeiro.Aula.Api.Configuration;
using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Infra.Context;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinanceiroDb>(options => options.UseSqlServer("Data Source=localhost;Initial Catalog=ApiAula;Persist Security Info=True;Encrypt=False;User ID=sa;Password=feherr")); // TODO: colocar no appsettings

builder.Services
    .DeclareRepositorys()
    .DeclareServices()
    .DeclareDomainServices()
    .DeclareQueues()
    .DeclareApiServices(builder.Configuration);

var domainAssembly = AppDomain.CurrentDomain.Load("Financeiro.Aula.Domain");
builder.Services.AddMediatR(domainAssembly);

builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.Authority = "http://localhost:8080/realms/myrealm";
    o.Audience = "account";
    o.RequireHttpsMetadata = false;

    o.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();

            c.Response.StatusCode = 500;
            c.Response.ContentType = "text/plain";
            //if (Environment.IsDevelopment())
            //{
            //return c.Response.WriteAsync(c.Exception.ToString());
            Console.WriteLine(c.Exception.ToString());
            return Task.FromResult(string.Empty);
            //}
            //return c.Response.WriteAsync("An error occured processing your authentication.");
        }
    };
});

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
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

//app.UseHttpsRedirection();

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
