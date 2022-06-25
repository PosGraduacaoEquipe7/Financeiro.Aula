# Financeiro

Projeto da cadeira de **Projeto e Arquitetura de Software** da Pós de Engenharia de Software da Unisinos - 2022.

## Docker

### Executar imagem
```
docker run -it -p 5000:5000 -p 5001:5001 -e ASPNETCORE_HTTP_PORT=https://+:5001 -e ASPNETCORE_URLS=http://+:5000 -e ASPNETCORE_ENVIRONMENT=Development  ghcr.io/posgraduacaoequipe7/financeiro.aula:v1
```

### Baixar imagem
```
docker pull ghcr.io/posgraduacaoequipe7/financeiro.aula:v1
```

### Gerar imagem local e executar
```
docker build . -t financeiro_aula
docker run -it -p 5000:5000 -p 5001:5001 -e ASPNETCORE_HTTP_PORT=https://+:5001 -e ASPNETCORE_URLS=http://+:5000 -e ASPNETCORE_ENVIRONMENT=Development financeiro_aula
```
