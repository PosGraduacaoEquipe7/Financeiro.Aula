# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /source
COPY . .
RUN dotnet restore "Financeiro.Boleto.Api/Financeiro.Boleto.Api.csproj"
RUN dotnet publish "Financeiro.Boleto.Api/Financeiro.Boleto.Api.csproj" -c release -o /app --no-restore

# Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

ENV ASPNETCORE_URLS=http://+:5050
ENV ASPNETCORE_ENVIRONMENT=Development
ENV TZ=America/Sao_Paulo

EXPOSE 5050

ENTRYPOINT ["dotnet", "Financeiro.Boleto.Api.dll"]

