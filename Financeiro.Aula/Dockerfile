# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /source
COPY . .
RUN dotnet restore "Financeiro.Aula.Api/Financeiro.Aula.Api.csproj"
RUN dotnet publish "Financeiro.Aula.Api/Financeiro.Aula.Api.csproj" -c release -o /app --no-restore

# Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENV TZ=America/Sao_Paulo

EXPOSE 5000

ENTRYPOINT ["dotnet", "Financeiro.Aula.Api.dll"]

