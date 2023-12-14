using Financeiro.Auth.Configuration;
using Financeiro.Auth.Context;
using Financeiro.Auth.Entities;
using Financeiro.Auth.Interfaces.Repositories;
using Financeiro.Auth.Interfaces.Services;
using Financeiro.Auth.Repositories;
using Financeiro.Auth.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var _myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

var authConfiguration = builder.Configuration.GetSection("Auth").Get<AuthConfiguration>() ?? throw new NullReferenceException("'Auth' deve ser configurado no appsettings.json");
builder.Services.AddSingleton(authConfiguration);

builder.Services
    .AddScoped<IAcessoRepository, AcessoRepository>()
    .AddScoped<IUsuarioRepository, UsuarioRepository>()
    .AddScoped<ITokenService, TokenService>()
    .AddScoped<IAcessoService, AcessoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(_myAllowSpecificOrigins,
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

app.UseHttpsRedirection();

app.UseCors(_myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthDb>();
    db.Database.EnsureCreated();

    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new Usuario(0, "Felipe", "felipejunges@yahoo.com.br", "felipe123", "Admin"));

        db.SaveChanges();
    }
}

app.Run();
