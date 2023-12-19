using Financeiro.Aula.Api.Configuration;
using Financeiro.Aula.Domain;
using Financeiro.Aula.Infra.Context;
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

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(DomainAssemblyReference.Assembly));

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

app.EnsureDbCreated();

app.Run();
