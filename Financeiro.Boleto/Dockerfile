# Build
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal as build
WORKDIR /source
COPY . .
RUN dotnet restore "Financeiro.Boleto.Api/Financeiro.Boleto.Api.csproj"
RUN dotnet publish "Financeiro.Boleto.Api/Financeiro.Boleto.Api.csproj" -c release -o /app --no-restore

# Run
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

ENV ASPNETCORE_URLS=http://+:5050
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 5050

ENTRYPOINT ["dotnet", "Financeiro.Boleto.Api.dll"]

