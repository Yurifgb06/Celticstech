# Celticstech

## Plataforma Inteligente de Monitoramento Agrícola

O Celticstech é uma API REST desenvolvida para a Global Solution 2026 da disciplina Advanced Business Development with .NET.

O projeto aplica conceitos de Economia Espacial ao agronegócio, utilizando localização geográfica, coordenadas automáticas e dados climáticos em tempo real para auxiliar associações agrícolas e produtores rurais do Nordeste brasileiro na tomada de decisões preventivas.

A plataforma integra informações cadastrais, monitoramento climático e geração automática de diagnósticos agrícolas, permitindo identificar riscos e recomendar ações antes que ocorram perdas na produção.

---

# Integrantes

* João Victor Vendrameto – RM 563665 – 2TDSPV
* Nicolas de Oliveira Jacob – RM 564205 – 2TDSPX
* Gabriel Ambrósio Saraiva – RM 566552 – 2TDSPV
* Vinicius Romaguera Cardozo – RM 562308 – 2TDSPX
* Yuri Fuzinatto Garzoli Barreto – RM 561450 – 2TDSPX

---

# Tecnologias Utilizadas

Backend

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* Swagger / OpenAPI
* Health Checks

Frontend

* HTML5
* CSS3
* JavaScript
* Bootstrap 5
* Chart.js

Infraestrutura

* Docker
* Docker Compose

Integrações

* Open-Meteo API

---

# Arquitetura da Solução

Frontend (Bootstrap + JavaScript)

↓

ASP.NET Core Web API (.NET 8)

↓

Entity Framework Core

↓

PostgreSQL

Integrações Externas:

* Open-Meteo API
* Health Check

---

# Objetivo do Projeto

O sistema foi criado para auxiliar associações agrícolas do Nordeste brasileiro através da análise de condições climáticas em tempo real.

Utilizando informações de temperatura, umidade, chuva e velocidade do vento, o sistema calcula um Score de Risco e gera recomendações técnicas específicas para cada cultivo monitorado.

---

# Funcionalidades

## Regiões

Permite cadastrar regiões agrícolas monitoradas.

Ao informar apenas:

* Nome da região
* UF

o sistema preenche automaticamente:

* Latitude
* Longitude

através do serviço interno de coordenadas.

---

## Associações

Permite cadastrar cooperativas e associações agrícolas vinculadas a uma região.

Cada associação possui:

* Nome
* Sigla
* CNPJ
* Login
* Senha
* Região vinculada

---

## Cultivos

Permite cadastrar culturas agrícolas monitoradas.

Exemplos:

* Milho
* Mandioca
* Caju
* Feijão

Cada cultivo possui características específicas utilizadas na geração dos diagnósticos.

---

## Agricultores

Permite cadastrar produtores rurais vinculados a:

* Associação
* Cultivo

Exemplo de retorno:

```json
{
  "idAgricultor": 1,
  "nomeAgricultor": "José Ferreira da Silva",
  "nomeAssociacao": "Cooperativa Agro Pernambuco",
  "nomeCultivo": "Milho"
}
```

---

## Consulta Climática

O sistema consulta a Open-Meteo API utilizando as coordenadas da região cadastrada.

Dados obtidos:

* Temperatura
* Umidade
* Chuva
* Velocidade do vento

Endpoint:

```http
GET /api/Satelite/clima/regiao/{id}
```

---

## Diagnóstico Climático Inteligente

Principal funcionalidade do sistema.

O diagnóstico considera:

* Região
* Cultivo
* Temperatura
* Umidade
* Chuva
* Velocidade do vento

Endpoint:

```http
GET /api/Diagnostico/regiao/{idRegiao}/cultivo/{idCultivo}
```

Exemplo:

```http
GET /api/Diagnostico/regiao/1/cultivo/1
```

O sistema calcula:

* Score de risco
* Nível de risco
* Prioridade
* Prazo sugerido
* Ações recomendadas

Além disso, gera automaticamente uma orientação técnica completa.

---

# Score de Risco

O Score de Risco varia de:

```text
0 a 100
```

Classificação:

```text
0 a 39   → BAIXO

40 a 69  → MODERADO

70 a 100 → ALTO
```

Fatores avaliados:

* Temperatura
* Umidade
* Chuva
* Velocidade do vento

---

# Recomendação Técnica

Cada diagnóstico gera automaticamente:

* Resumo do risco
* Motivo do risco
* Ações recomendadas
* Prioridade
* Prazo sugerido
* Observação técnica
* Fonte dos dados

As recomendações são armazenadas integralmente no banco de dados utilizando o tipo TEXT, permitindo salvar orientações detalhadas sem limitação prática de tamanho.

---

# Histórico Climático

Cada diagnóstico gerado é salvo automaticamente.

Endpoint:

```http
GET /api/Recomendacoes/historico-climatico
```

O histórico exibe:

* Associação
* Cultivo
* Data
* Temperatura
* Umidade
* Chuva
* Vento
* Score de risco
* Nível de risco
* Orientação
* Fonte dos dados

---

# Dashboard

Endpoint:

```http
GET /api/Dashboard/resumo
```

Exemplo:

```json
{
  "totalRegioes": 5,
  "totalAssociacoes": 3,
  "totalCultivos": 5,
  "totalAgricultores": 8,
  "totalRecomendacoes": 20,
  "integracaoOpenMeteo": "Ativa",
  "statusSistema": "Operacional"
}
```

---

# Coordenadas Automáticas

UFs suportadas:

```text
BA
PE
CE
MA
PI
RN
PB
AL
SE
```

Exemplo:

```json
{
  "nomeRegiao": "Pernambuco",
  "ufRegiao": "PE"
}
```

Resultado:

```json
{
  "latitude": -8.8137,
  "longitude": -36.9541
}
```

---

# Endpoints Principais

```text
/api/Regioes
/api/Associacoes
/api/Cultivos
/api/Agricultores
/api/Recomendacoes
/api/Recomendacoes/historico-climatico
/api/Satelite/clima/regiao/{id}
/api/Diagnostico/regiao/{idRegiao}/cultivo/{idCultivo}
/api/Dashboard/resumo
/health
```

---

# Como Executar o Projeto

## 1. Configurar o PostgreSQL

Editar o arquivo:

```text
appsettings.json
```

Exemplo:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=CelticstechDb;Username=postgres;Password=123456"
}
```

---

## 2. Restaurar Dependências

```bash
dotnet restore
```

---

## 3. Executar a Aplicação

Na raiz do projeto:

```bash
dotnet run
```

O Entity Framework Core aplica automaticamente as migrations pendentes durante a inicialização da aplicação.

---

## 4. Acessar a Aplicação

Utilize a URL exibida pelo terminal ou Visual Studio.

Exemplos:

```text
http://localhost:5000
https://localhost:5001
https://localhost:7143
```

---

## 5. Swagger

A documentação da API estará disponível em:

```text
http://localhost:5000/swagger
```

ou na porta exibida pelo terminal.

No Swagger é possível:

* Visualizar todos os endpoints
* Testar operações CRUD
* Consultar modelos da API
* Validar respostas em tempo real

---

## 6. Health Check

Endpoint:

```http
GET /health
```

Exemplo:

```text
http://localhost:5000/health
```

Retorno esperado:

```text
Healthy
```

Status esperado:

```text
200 OK
```

---

## 7. Frontend

O frontend é servido automaticamente pelo ASP.NET Core através da pasta:

```text
wwwroot
```

Não é necessário executar outro servidor.

Acesse:

```text
http://localhost:5000
```

ou a porta informada pelo terminal.

Funcionalidades disponíveis:

* Dashboard
* Regiões
* Associações
* Cultivos
* Agricultores
* Consulta Climática
* Diagnóstico Climático
* Histórico Climático
* Tema Escuro
* Toasts de Notificação

---

## 8. Fluxo Recomendado de Validação

Validar os seguintes endpoints:

```http
GET /health

GET /api/Dashboard/resumo

GET /api/Satelite/clima/regiao/1

GET /api/Diagnostico/regiao/1/cultivo/1

GET /api/Recomendacoes/historico-climatico
```

Se todos responderem corretamente, a aplicação está operacional.


Recursos disponíveis:

* Dashboard
* Regiões
* Associações
* Cultivos
* Agricultores
* Consulta Climática
* Diagnóstico Climático
* Histórico Climático
* Tema Escuro
* Toasts de Notificação

---

# Docker

Executar:

```bash
docker compose up -d --build
```

Acessos:

Swagger:

```text
http://localhost:8080/swagger
```

Frontend:

```text
http://localhost:8080
```

Health Check:

```text
http://localhost:8080/health
```

---

# Modelagem do Banco de Dados

A modelagem completa utilizada no projeto pode ser visualizada abaixo:

![Modelagem do Banco](Celticstech/Images/Modelagem_Banco.jpeg)

---

# Status do Projeto

Funcionalidades concluídas:

* CRUD completo
* PostgreSQL
* Entity Framework Core
* Swagger/OpenAPI
* Docker
* Dashboard
* Frontend Responsivo
* Integração Open-Meteo
* Diagnóstico Climático Inteligente
* Histórico Climático
* Health Check
* Tema Escuro
* Economia Espacial aplicada ao Agronegócio
