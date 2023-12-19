using Financeiro.Aula.Api.Services;
using Financeiro.Aula.Domain.Commands.Clientes.IncluirCliente;
using Financeiro.Aula.Domain.Configurations;
using Financeiro.Aula.Domain.Entities;
using Financeiro.Aula.Domain.Interfaces.ApiServices;
using Financeiro.Aula.Domain.Interfaces.Cache;
using Financeiro.Aula.Domain.Interfaces.DomainServices;
using Financeiro.Aula.Domain.Interfaces.Queues;
using Financeiro.Aula.Domain.Interfaces.Repositories;
using Financeiro.Aula.Domain.Interfaces.Services;
using Financeiro.Aula.Domain.Interfaces.Services.CEPs;
using Financeiro.Aula.Domain.Interfaces.Services.PDFs;
using Financeiro.Aula.Domain.Services.Cache;
using Financeiro.Aula.Domain.Services.DomainServices;
using Financeiro.Aula.Domain.Services.PDFs;
using Financeiro.Aula.Infra.Context;
using Financeiro.Aula.Infra.Repositories;
using Financeiro.Aula.Infra.Services.ApiServices;
using Financeiro.Aula.Infra.Services.Queues;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Net.Http.Headers;
using System.Text;

namespace Financeiro.Aula.Api.Configuration
{
    public static class DependencyInjection
    {
        public static IHost EnsureDbCreated(this IHost app)
        {
            int tentativa = 0;

            var logger = app.Services.GetService<ILogger<Program>>();

            while (tentativa < 10)
            {
                tentativa++;

                logger?.LogInformation("Aguardando {tempo} segundos para criar FinanceiroDb", tentativa);
                Thread.Sleep(tentativa * 1000);

                try
                {
                    var scope = app.Services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<FinanceiroDb>();
                    db.Database.EnsureCreated();

                    CriarDadosIniciais(db);

                    logger?.LogInformation("Criada database FinanceiroDb");

                    return app;
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "Não foi possível conectar ao FinanceiroDb Context. Tentativa {tentativa}", tentativa);
                }
            }

            throw new DataException("Não foi possível conectar no FinanceiroDb depois de 10 tentativas");
        }

        private static void CriarDadosIniciais(FinanceiroDb db)
        {
            if (!db.Cursos.Any())
            {
                var curso = new Curso(
                    id: 0,
                    descricao: "Nutri��o",
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

        public static IServiceCollection DeclareRepositorys(this IServiceCollection services)
        {
            services
                .AddScoped<IClienteRepository, ClienteRepository>()
                .AddScoped<IContratoRepository, ContratoRepository>()
                .AddScoped<ICursoRepository, CursoRepository>()
                .AddScoped<IEmpresaRepository, EmpresaMockRepository>()
                .AddScoped<IParcelaRepository, ParcelaRepository>()
                .AddScoped<ITurmaRepository, TurmaRepository>();

            return services;
        }

        public static IServiceCollection DeclareServices(this IServiceCollection services)
        {
            services
                .AddScoped<IGeradorContratoPdfService, GeradorContratoPdfService>()
                .AddScoped<ICacheService, CacheService>();

            return services;
        }

        public static IServiceCollection DeclareDomainServices(this IServiceCollection services)
        {
            services
                .AddScoped<ICepService, CepService>()
                .AddScoped<IClienteService, ClienteService>()
                .AddScoped<IParcelaService, ParcelaService>();

            return services;
        }

        public static IServiceCollection DeclareValidators(this IServiceCollection services)
        {
            services
                .AddScoped<IValidator<IncluirClienteCommand>, IncluirClienteCommandValidator>();

            return services;
        }

        public static IServiceCollection DeclareQueues(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfig"))
                .AddScoped<IRegistrarBoletoQueue, RegistrarBoletoQueue>()
                .AddScoped<IBoletoRegistradoQueueConsumer, BoletoRegistradoQueueConsumer>();

            services.AddHostedService<BoletoRegistradoService>();

            return services;
        }

        public static IServiceCollection DeclareApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IAuthService, AuthService>();

            services.AddHttpClient<IBoletoApiService, BoletoApiService>((service, client) =>
            {
                var context = service.GetRequiredService<IHttpContextAccessor>().HttpContext!;

                client.BaseAddress = new Uri(configuration["ApiBoleto:BaseAddress"] ?? string.Empty);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{RemoverBearer(context.Request.Headers["Authorization"])}");
            });

            services.AddHttpClient<ICepApiService, ViaCEPApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration["ApiViaCEP:BaseAddress"] ?? string.Empty);
            });

            return services;
        }

        public static IServiceCollection AddApiSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            services.AddSwaggerGen(option =>
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

            return services;
        }

        public static IServiceCollection AddApiSeqLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSeq(configuration.GetSection("SeqLog"));
            });

            return services;
        }

        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var keySecret = configuration.GetSection("Auth:KeySecret")?.Value ?? throw new NullReferenceException("'Auth:KeySecret' deve ser configurado no appsettings.json");
            var secret = Encoding.ASCII.GetBytes(keySecret);

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static IServiceCollection AddKeyCloakAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.Authority = "http://keycloak:8085/realms/myrealm"; // TODO: appsettings
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

            return services;
        }

        private static string? RemoverBearer(string? authorization)
        {
            if (authorization is null) return null;
            if (string.IsNullOrEmpty(authorization)) return string.Empty;

            if (authorization.StartsWith("Bearer ")) return authorization.Substring(7);

            return authorization;
        }
    }
}