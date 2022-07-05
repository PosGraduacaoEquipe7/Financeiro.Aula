# Financeiro

Projeto da cadeira de **Projeto e Arquitetura de Software** da Pós de Engenharia de Software da Unisinos - 2022.

## Integrantes

* Airton Felipe Sauer de Oliveira
* Andressa Magnus Friedrichs
* Felipe Amadeu Junges
* Marcelo Aloísio da Silva

## Docker

A imagem Docker está disponível no registry: ghcr.io/posgraduacaoequipe7/financeiro.aula:v2

### Baixar imagem
```
docker pull ghcr.io/posgraduacaoequipe7/financeiro.aula:v2
```

### Executar imagem
```
docker run -it -p 5000:5000 ghcr.io/posgraduacaoequipe7/financeiro.aula:v2
```

### Rodar projeto no navegador
http://localhost:5000/swagger

### Se preferir gerar imagem local e executar
```
docker build . -t financeiro.aula
docker run -it -p 5000:5000 financeiro.aula
```
