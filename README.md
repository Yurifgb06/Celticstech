# Celticstech

## Integrantes

- João Victor Vendrameto - RM 563665 - 2TDSPV
- Nicolas de Oliveira Jacob - RM 564205 - 2TDSPX
- Gabriel Ambrósio Saraiva - RM 566552 - 2TDSPV
- Vinicius Romaguera Cardozo - RM 562308 - 2TDSPX
- Yuri Fuzinatto Garzoli Barreto - RM 561450 - 2TDSPX

---

# Objetivo do Projeto

O Celticstech foi desenvolvido para auxiliar associações agrícolas da região Nordeste no gerenciamento de cultivos e recomendações agrícolas.

A aplicação permite cadastrar regiões, associações, agricultores, cultivos e recomendações, mantendo o relacionamento entre as entidades e facilitando a tomada de decisão.

---

# Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Swagger
- Docker
- Docker Compose

---

# Funcionalidades

- Cadastro de Regiões
- Cadastro de Associações
- Cadastro de Agricultores
- Cadastro de Cultivos
- Cadastro de Recomendações
- Relacionamento entre as entidades
- Geração automática de recomendações agrícolas
- Validação de dados
- Health Check
- Documentação da API com Swagger
- Execução em containers Docker

---

# Estrutura do Projeto

```text
Controllers
Models
DTOs
Data
Migrations
Properties
Dockerfile
docker-compose.yml
Program.cs
appsettings.json
```

---

# Modelagem do Banco

Inserir abaixo a imagem da modelagem utilizada no projeto.

![Modelagem do Banco](Celticstech/Images/Modelagem_Banco.jpeg)

---

# Relacionamentos

A aplicação utiliza relacionamentos entre as entidades para garantir a integridade dos dados.

Exemplos:

- Uma Região pode possuir várias Associações.
- Uma Associação pode possuir várias Recomendações.
- Um Cultivo pode possuir várias Recomendações.

Relacionamento utilizado:

```text
1:N (Um para Muitos)
```

---

# Recomendação Automática

O sistema gera automaticamente recomendações agrícolas de acordo com o cultivo informado.

O usuário informa apenas:

- Data da recomendação
- Associação
- Cultivo

A API gera automaticamente:

- Tipo da recomendação
- Orientação

Exemplos:

- Milho → Irrigação
- Soja → Monitoramento de umidade
- Caju → Atenção aos períodos de seca
- Cana-de-açúcar → Controle de irrigação

Dessa forma o usuário não precisa cadastrar manualmente as recomendações.

---

# Segurança

A entidade Associação possui campo de senha para cadastro.

Porém, por questões de segurança, a senha não é retornada nos endpoints de consulta (GET).

---

# Banco de Dados

Banco utilizado:

```text
PostgreSQL
```

A persistência dos dados é realizada através do Entity Framework Core.

As tabelas são criadas através das migrations.

---

# Migrations

Para criar uma migration:

```powershell
Add-Migration NomeDaMigration
```

Para atualizar o banco:

```powershell
Update-Database
```

---

# Executando o Projeto

## Executando Localmente

1. Abrir o projeto no Visual Studio.
2. Configurar a conexão com PostgreSQL.
3. Executar:

```powershell
Update-Database
```

4. Iniciar a aplicação.

Swagger:

```text
https://localhost:7113/swagger
```

Health Check:

```text
https://localhost:7113/health
```

---

# Executando com Docker

Na pasta do projeto:

```bash
docker compose up --build
```

Swagger:

```text
http://localhost:8080/swagger
```

Health Check:

```text
http://localhost:8080/health
```

---

# Testes Realizados

Foram realizados testes completos utilizando o Swagger.

### Regiões

- Create
- Read
- Update
- Delete

### Associações

- Create
- Read
- Update
- Delete

### Agricultores

- Create
- Read
- Update
- Delete

### Cultivos

- Create
- Read
- Update
- Delete

### Recomendações

- Create
- Read
- Update
- Delete

Também foram realizados testes de:

- Relacionamentos
- Health Check
- Docker
- Persistência em banco de dados

---

# Endpoints Principais

```text
/api/Regioes
/api/Associacoes
/api/Agricultores
/api/Cultivos
/api/Recomendacoes
/health
```

---

