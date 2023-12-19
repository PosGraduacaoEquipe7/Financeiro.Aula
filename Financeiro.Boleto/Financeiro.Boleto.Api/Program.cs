using Financeiro.Boleto.Api.Configuration;
using Financeiro.Boleto.Api.Services;
using Financeiro.Boleto.Domain.Configuration;
using Financeiro.Boleto.Domain.Interfaces.ApiServices;
using Financeiro.Boleto.Domain.Interfaces.Queues;
using Financeiro.Boleto.Domain.Interfaces.Repositories;
using Financeiro.Boleto.Domain.Interfaces.Services;
using Financeiro.Boleto.Domain.Services.ApiServices;
using Financeiro.Boleto.Infra.Context;
using Financeiro.Boleto.Infra.Repositories;
using Financeiro.Boleto.Infra.Services.ApiServices;
using Financeiro.Boleto.Infra.Services.Queues;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BoletoDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
            .AddScoped<IBoletoService, BoletoService>()
            .AddScoped<IBoletoRepository, BoletoRepository>()
            .AddScoped<IParametroBoletoRepository, ParametroBoletoRepository>()
            .Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfig"))
            .AddScoped<IBoletoRegistradoQueue, BoletoRegistradoQueue>()
            .AddScoped<IRegistrarBoletoQueueConsumer, RegistrarBoletoQueueConsumer>();

builder.Services.AddHostedService<RegistrarBoletoService>();

builder.Services.AddHttpClient<IGeradorBoletoApiService, BoletoCloudApiService>(client =>
{
    var token = $"{builder.Configuration["ApiBoletoCloud:ApiKey"]}:token";

    var boletoCloudUrl = builder.Configuration["ApiBoletoCloud:BaseAddress"] ?? throw new NullReferenceException("'ApiBoletoCloud:BaseAddress' deve ser configurado no appsettings.json");

    client.BaseAddress = new Uri(boletoCloudUrl);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(token)));
});

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSeq(builder.Configuration.GetSection("SeqLog"));
});

builder.Services.AddControllers();

//builder.Services.AddKeyCloakAuthentication();
builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
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

//app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.EnsureDbCreated();

app.Run();
