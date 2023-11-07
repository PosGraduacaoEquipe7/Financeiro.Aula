using Financeiro.Auth.Interfaces.Repositories;
using Financeiro.Auth.Interfaces.Services;
using Financeiro.Auth.Repositories;
using Financeiro.Auth.Services;
using System.Reflection;
using System.Text.Json.Serialization;

Assembly domainAssembly = AppDomain.CurrentDomain.Load("Financeiro.Auth");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(domainAssembly);
});

builder.Services
    .AddScoped<IUsuarioRepository, UsuarioMockRepository>()
    .AddScoped<ITokenService, TokenService>();

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
