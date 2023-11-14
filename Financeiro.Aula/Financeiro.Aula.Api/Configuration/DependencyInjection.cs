using Financeiro.Aula.Api.Services;
using Financeiro.Aula.Domain.Configurations;
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
using Financeiro.Aula.Infra.Repositories;
using Financeiro.Aula.Infra.Services.ApiServices;
using Financeiro.Aula.Infra.Services.Queues;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;
using System.Text;

namespace Financeiro.Aula.Api.Configuration
{
    public static class DependencyInjection
    {
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

        public static IServiceCollection DeclareQueues(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfig"))
                .AddScoped<IRegistrarBoletoQueue, RegistrarBoletoQueue>();

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

        public static IServiceCollection AddApiAuthentication(this IServiceCollection services)
        {
            var secret = Encoding.ASCII.GetBytes("fedaf7d8863b48e197b9287d492b708e"); // TODO: parâmetro

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

        public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSeq(configuration.GetSection("Serilog"));
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