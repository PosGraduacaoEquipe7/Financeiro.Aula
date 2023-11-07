# Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /source
COPY . .
RUN dotnet restore "Financeiro.Auth.csproj"
RUN dotnet publish "Financeiro.Auth.csproj" -c release -o /app --no-restore

# Run
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./

ENV ASPNETCORE_URLS=http://+:5112
ENV ASPNETCORE_ENVIRONMENT=Development
ENV TZ=America/Sao_Paulo

EXPOSE 5112

ENTRYPOINT ["dotnet", "Financeiro.Auth.dll"]

