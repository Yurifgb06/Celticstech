# Validação do Projeto Celticstech

## 1. Build do Projeto

Executar:

```bash
dotnet build
```

Resultado esperado:

```text
0 erros
0 avisos
```

---

## 2. Executar a Aplicação

Executar:

```bash
dotnet run
```

ou:

```bash
dotnet run --project Celticstech/Celticstech.csproj
```

Resultado esperado:

```text
Aplicação iniciada sem erros.
```

Abrir a URL exibida no terminal.

Exemplos:

```text
http://localhost:5000
https://localhost:5001
https://localhost:7143
```

---

## 3. Health Check

Endpoint:

```http
GET /health
```

Exemplo:

```text
http://localhost:5000/health
```

Resultado esperado:

```text
Healthy
```

Status esperado:

```text
200 OK
```

---

## 4. Swagger

Abrir:

```text
http://localhost:5000/swagger
```

ou a porta informada pelo terminal.

Validar se aparecem os controllers:

```text
RegioesController
AssociacoesController
CultivosController
AgricultoresController
RecomendacoesController
SateliteController
DiagnosticoController
DashboardController
```

---

## 5. Dashboard

Endpoint:

```http
GET /api/Dashboard/resumo
```

Resultado esperado:

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

Observação:

Os números podem variar conforme a quantidade de dados cadastrados.

---

## 6. Cadastro de Região

Endpoint:

```http
POST /api/Regioes
```

Body:

```json
{
  "nomeRegiao": "Pernambuco",
  "ufRegiao": "PE"
}
```

Resultado esperado:

```json
{
  "idRegiao": 1,
  "nomeRegiao": "Pernambuco",
  "ufRegiao": "PE",
  "latitude": -8.8137,
  "longitude": -36.9541
}
```

Validar:

```text
Região cadastrada.
Latitude preenchida automaticamente.
Longitude preenchida automaticamente.
UF aceita pelo sistema.
```

---

## 7. Listagem de Regiões

Endpoint:

```http
GET /api/Regioes
```

Resultado esperado:

```text
Lista de regiões cadastradas.
```

---

## 8. Atualização de Região

Endpoint:

```http
PUT /api/Regioes/1
```

Body:

```json
{
  "nomeRegiao": "Bahia",
  "ufRegiao": "BA"
}
```

Resultado esperado:

```text
Região atualizada com sucesso.
Coordenadas recalculadas automaticamente.
```

---

## 9. Cadastro de Associação

Endpoint:

```http
POST /api/Associacoes
```

Body:

```json
{
  "nomeAssociacao": "Cooperativa Agro Pernambuco",
  "siglaAssociacao": "COOAPE",
  "idRegiao": 1,
  "cnpj": "12.345.678/0001-90",
  "login": "cooape",
  "senha": "Senha123"
}
```

Resultado esperado:

```text
Associação cadastrada com sucesso.
```

Validar:

```text
Associação vinculada à região.
CNPJ aceito.
Login e senha cadastrados.
```

---

## 10. Listagem de Associações

Endpoint:

```http
GET /api/Associacoes
```

Resultado esperado:

```text
Lista de associações cadastradas.
```

---

## 11. Cadastro de Cultivo

Endpoint:

```http
POST /api/Cultivos
```

Body:

```json
{
  "nomeCultivo": "Milho",
  "categoriaCultivo": "Grãos",
  "porteCultivo": "ARBUSTO",
  "tempoColheita": "90 a 120 dias",
  "vidaUtil": "1 safra",
  "intermitencia": "Sazonal"
}
```

Resultado esperado:

```text
Cultivo cadastrado com sucesso.
```

---

## 12. Listagem de Cultivos

Endpoint:

```http
GET /api/Cultivos
```

Resultado esperado:

```text
Lista de cultivos cadastrados.
```

---

## 13. Cadastro de Agricultor

Endpoint:

```http
POST /api/Agricultores
```

Body:

```json
{
  "nomeAgricultor": "José Ferreira da Silva",
  "cpf": "987.654.321-00",
  "telefone": "(71) 99123-4567",
  "idAssociacao": 1,
  "idCultivo": 1
}
```

Resultado esperado:

```json
{
  "idAgricultor": 1,
  "nomeAgricultor": "José Ferreira da Silva",
  "nomeAssociacao": "Cooperativa Agro Pernambuco",
  "nomeCultivo": "Milho"
}
```

Validar:

```text
Agricultor vinculado à associação.
Agricultor vinculado ao cultivo.
CPF formatado aceito.
Telefone formatado aceito.
```

---

## 14. Listagem de Agricultores

Endpoint:

```http
GET /api/Agricultores
```

Resultado esperado:

```text
Lista de agricultores cadastrados com associação e cultivo vinculados.
```

---

## 15. Consulta Climática Open-Meteo

Endpoint:

```http
GET /api/Satelite/clima/regiao/1
```

Resultado esperado:

```json
{
  "idRegiao": 1,
  "regiao": "Pernambuco",
  "uf": "PE",
  "temperatura": 29,
  "umidade": 72,
  "velocidadeVento": 14,
  "chuva": 0,
  "scoreRisco": 75,
  "nivelRisco": "ALTO",
  "fonteDados": "Open-Meteo"
}
```

Validar:

```text
Dados reais retornando.
Temperatura preenchida.
Umidade preenchida.
Vento preenchido.
Chuva preenchida.
Fonte como Open-Meteo.
```

---

## 16. Diagnóstico Climático

Endpoint:

```http
GET /api/Diagnostico/regiao/1/cultivo/1
```

Resultado esperado:

```text
REGIÃO:
Pernambuco

CULTIVO:
Milho

RESUMO DO RISCO:
Risco calculado para o cultivo monitorado.

MOTIVO:
Análise baseada em temperatura, umidade, chuva e velocidade do vento.

AÇÕES RECOMENDADAS:
1. Monitorar umidade do solo.
2. Ajustar irrigação conforme necessidade.
3. Evitar pulverizações em períodos de vento forte.

PRIORIDADE:
Moderada ou Alta

PRAZO SUGERIDO:
24 a 48 horas

OBSERVAÇÃO TÉCNICA:
Análise calculada com dados climáticos em tempo real.

FONTE:
Open-Meteo API
```

Validar:

```text
Retorna região.
Retorna cultivo.
Retorna score de risco.
Retorna nível de risco.
Retorna orientação completa.
Salva recomendação no histórico climático.
```

---

## 17. Histórico Climático

Endpoint:

```http
GET /api/Recomendacoes/historico-climatico
```

Resultado esperado:

```json
[
  {
    "associacao": "Cooperativa Agro Pernambuco",
    "cultivo": "Milho",
    "scoreRisco": 75,
    "nivelRisco": "ALTO",
    "temperatura": 29,
    "umidade": 72,
    "chuva": 0,
    "velocidadeVento": 14,
    "fonteDados": "Open-Meteo API",
    "orientacao": "REGIÃO:\nPernambuco\n\nCULTIVO:\nMilho..."
  }
]
```

Validar:

```text
Histórico carrega.
Diagnóstico gerado aparece no histórico.
Modal de detalhes abre no frontend.
Orientação completa aparece.
Quebras de linha aparecem corretamente.
```

---

## 18. CSS do Modal do Histórico

Validar se a área da orientação possui:

```css
white-space: pre-line;
```

Exemplo:

```css
.orientacao-detalhe,
#orientacaoDetalhe,
.modal-orientacao {
    white-space: pre-line;
}
```

Resultado esperado:

```text
A orientação deve aparecer com quebras de linha, e não em uma única linha.
```

---

## 19. CRUD de Recomendações

Endpoint:

```http
GET /api/Recomendacoes
```

Resultado esperado:

```text
Lista de recomendações cadastradas.
```

Validar:

```text
Recomendações aparecem corretamente.
Associação aparece.
Cultivo aparece.
Orientação aparece.
Nível de risco aparece.
```

---

## 20. Frontend

Abrir:

```text
http://localhost:5000
```

ou a porta exibida no terminal.

Validar telas:

```text
Dashboard
Regiões
Associações
Cultivos
Agricultores
Recomendações
Consulta Climática
Diagnóstico Climático
Histórico Climático
Validação do Sistema
```

Validar funcionalidades:

```text
Cadastrar
Listar
Atualizar
Excluir
Abrir modais
Exibir toasts
Alternar tema escuro
Consultar clima
Gerar diagnóstico
Visualizar histórico
```

---

## 21. Toasts

Validar mensagens:

Ao cadastrar:

```text
Cadastro realizado com sucesso.
```

Ao atualizar:

```text
Registro atualizado com sucesso.
```

Ao excluir:

```text
Registro removido com sucesso.
```

Ao ocorrer erro:

```text
Erro ao processar solicitação.
```

---

## 22. Tema Escuro

Validar:

```text
Tema escuro ativa corretamente.
Tema claro retorna corretamente.
Preferência fica salva no navegador.
```

---

## 23. Docker

Executar:

```bash
docker compose up -d --build
```

Validar containers:

```bash
docker ps
```

Acessar:

```text
http://localhost:8080
http://localhost:8080/swagger
http://localhost:8080/health
```

Resultado esperado no Health Check:

```text
Healthy
```

---