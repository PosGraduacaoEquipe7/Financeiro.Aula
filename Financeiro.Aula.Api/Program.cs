using Financeiro.Aula.Api.Configuration;
using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.ValueObjects;
using Financeiro.Aula.Infra.Context;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinanceiroDb>(options => options.UseSqlite("DataSource=Db/Financeiro.db"));

builder.Services
    .DeclareRepositorys()
    .DeclareServices()
    .DeclareDomainServices()
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
            return Task.FromResult("oi");
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
                          policy.WithOrigins(
                                    "http://localhost:4200"//,
                                                           //"http://localhost:5000"
                                )
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                          //.AllowCredentials();
                          //policy.SetIsOriginAllowed(origin => true);
                          //policy.AllowAnyMethod();
                          //policy.AllowAnyHeader();
                          //policy.AllowCredentials();
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
