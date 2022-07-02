# Financeiro

Projeto da cadeira de **Projeto e Arquitetura de Software** da Pós de Engenharia de Software da Unisinos - 2022.

## Integrantes

* Airton Felipe Sauer de Oliveira
* Andressa Magnus Friedrichs
* Felipe Amadeu Junges
* Marcelo Aloísio da Silva

## Docker

imagem docker está disponível no registry ghcr.io/posgraduacaoequipe7/financeiro.aula:v1

### Baixar imagem
```
docker pull ghcr.io/posgraduacaoequipe7/financeiro.aula:v1
```

### Executar imagem
```
docker run -it -p 5000:5000 ghcr.io/posgraduacaoequipe7/financeiro.aula:v1
```

### Se preferir gerar imagem local e executar
```
docker build . -t financeiro_aula
docker run -it -p 5000:5000 financeiro_aula
```
