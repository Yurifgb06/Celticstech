# Celticstech

## Integrantes

* João Victor Vendrameto - RM 563665
* Nicolas de Oliveira Jacob - RM 564205
* Gabriel Ambrósio Saraiva - RM 566552
* Vinicius Romaguera Cardozo - RM 562308
* Yuri Fuzinatto Garzoli Barreto - RM 561450

---

## Repositório

GitHub:

https://github.com/Yurifgb06/Celticstech

---

## Sobre o Projeto

O Celticstech é uma API REST desenvolvida em .NET 8 com o objetivo de auxiliar associações agrícolas da região Nordeste do Brasil.

A aplicação permite o gerenciamento de regiões, associações, agricultores, cultivos e recomendações agrícolas, utilizando PostgreSQL como banco de dados e Entity Framework Core para persistência dos dados.

Além do CRUD completo, o sistema gera recomendações automáticas com base no cultivo informado, auxiliando no planejamento agrícola.

---

## Tecnologias Utilizadas

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* Swagger / OpenAPI
* Docker
* Docker Compose

---

## Estrutura do Projeto

* Controllers
* DTOs
* Models
* Data
* Migrations

---

## Relacionamentos

* Uma Região pode possuir várias Associações.
* Uma Associação pode possuir várias Recomendações.
* Um Cultivo pode possuir várias Recomendações.

---

## Regra de Negócio

O sistema gera automaticamente recomendações agrícolas de acordo com o cultivo informado.

Exemplos:

* Milho → Irrigação
* Soja → Irrigação
* Algodão → Colheita
* Caju → Não Irrigar
* Cana-de-açúcar → Não Irrigar

Caso não exista uma regra específica cadastrada, o sistema gera uma recomendação genérica baseada em monitoramento climático e irrigação.

---

## Modelagem do Banco de Dados

![Modelagem do Banco](Celticstech/Images/Modelagem_Banco.jpeg)

---

## Como Executar o Projeto

### Configurar Banco de Dados

Ajuste a Connection String no arquivo:

```text
appsettings.json
```

### Aplicar Migrations

```powershell
Update-Database
```

### Executar Aplicação

```powershell
dotnet run
```

### Executar com Docker

```powershell
docker compose up --build
```

---

## Endpoints

### Regiões

* GET /api/Regioes
* GET /api/Regioes/{id}
* POST /api/Regioes
* PUT /api/Regioes/{id}
* DELETE /api/Regioes/{id}

### Associações

* GET /api/Associacoes
* GET /api/Associacoes/{id}
* POST /api/Associacoes
* PUT /api/Associacoes/{id}
* DELETE /api/Associacoes/{id}

### Agricultores

* GET /api/Agricultores
* GET /api/Agricultores/{id}
* POST /api/Agricultores
* PUT /api/Agricultores/{id}
* DELETE /api/Agricultores/{id}

### Cultivos

* GET /api/Cultivos
* GET /api/Cultivos/{id}
* POST /api/Cultivos
* PUT /api/Cultivos/{id}
* DELETE /api/Cultivos/{id}

### Recomendações

* GET /api/Recomendacoes
* GET /api/Recomendacoes/{id}
* POST /api/Recomendacoes
* PUT /api/Recomendacoes/{id}
* DELETE /api/Recomendacoes/{id}

---

## Testes Realizados

### Regiões

* Cadastro de regiões.
* Consulta por ID.
* Consulta geral.
* Atualização de registros.
* Remoção de registros.

### Associações

* Cadastro de associações.
* Validação de região existente.
* Consulta por ID.
* Consulta geral.
* Atualização de registros.
* Remoção de registros.

### Agricultores

* Cadastro de agricultores.
* Consulta por ID.
* Consulta geral.
* Atualização de registros.
* Remoção de registros.

### Cultivos

* Cadastro de cultivos.
* Validação do porte do cultivo.
* Consulta por ID.
* Consulta geral.
* Atualização de registros.
* Remoção de registros.

### Recomendações

* Geração automática de recomendações.
* Validação de associação existente.
* Validação de cultivo existente.
* Consulta por ID.
* Consulta geral.
* Atualização de registros.
* Remoção de registros.

### Infraestrutura

* PostgreSQL.
* Entity Framework Migrations.
* Docker.
* Docker Compose.
* Health Check.

---

## Health Check

Endpoint para monitoramento da aplicação:

```http
GET /health
```

---

## Swagger

A documentação completa da API está disponível através do Swagger:

```text
/swagger
```
