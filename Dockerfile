# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal as build
WORKDIR /source
COPY . .
RUN dotnet restore "Financeiro.Aula.Api/Financeiro.Aula.Api.csproj"
RUN dotnet publish "Financeiro.Aula.Api/Financeiro.Aula.Api.csproj" -c release -o /app --no-restore

# Serve stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5001 5000

ENTRYPOINT ["dotnet", "Financeiro.Aula.Api.dll"]

