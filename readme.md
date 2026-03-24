# Desafio Gerenciador de Tarefas - API REST

Esta é uma API RESTful desenvolvida em ASP.NET Core (.NET 8) para o processamento e gestão de tarefas operacionais visando extensabilidade, suporte a alto volume de dados simultâneos e organização do código em boas práticas. 

## Tecnologias Utilizadas
* **Framework:** ASP.NET Core (.NET 8)
* **Linguagem:** C#
* **Banco de Dados:** PostgreSQL
* **ORM:** Entity Framework Core
* **Testes:** xUnit, Moq
* **Containerização:** Docker e Docker Compose

## Instruções de Instalação e Execução

O projeto está configurado para ser executado de forma simplificada através de containeres Docker, garantindo que o ambiente da base de dados e a API sejam normalizados automaticamente.

### Pré-requisitos
* Docker e Docker Compose instalados no sistema host.

### Passos para Execução via Docker
Os comandos a seguir são feitos via terminal Bash (Git Bash).

1. **Configuração de Variáveis de Ambiente:**
   Na raiz do projeto, copie o arquivo de exemplo para criar as variáveis definitivas:
   ```bash
   cp .env.example .env
   ```

2. **Iniciar os Contêineres:**
   Execute o comando abaixo na raiz do projeto para construir a imagem e iniciar os serviços:
   ```bash
   docker-compose up -d --build
   ```

3. **Acesso à Aplicação:**
   A API estará exposta e acessível na porta `5000` do host local (mapeada para a porta `8080` interna do contêiner).
   * **Swagger UI / Documentação:** http://localhost:5000/swagger

4. **Parar os Serviços:**
   ```bash
   docker-compose down
   ```

### Passos para Execução Local (Modo de Desenvolvimento)
Caso prefira rodar a API diretamente no host sem contentorizar a aplicação:
1. Garanta que uma instância do PostgreSQL está ativa na porta `5432` com as credenciais definidas em `appsettings.json`.
2. Navegue até a pasta `DesafioGerenciadorTarefas.Api`.
3. Execute `dotnet run`. A aplicação aplicará as migrações automaticamente na inicialização.

### Executar Testes Unitários
Na raiz do projeto:
```bash
dotnet test
```

## Exemplos de Requisições HTTP

Este guia apresenta exemplos de requisições HTTP para testar as funcionalidades do Gerenciador de Tarefas. Os exemplos utilizam a ferramenta `curl` e assumem que a API está sendo executada via Docker na porta **5000** (`http://localhost:5000`). 

> **Nota:** Se estiver executando localmente via Kestrel ou IIS Express, substitua a porta conforme definido no `launchSettings.json` (ex: 5011 ou 7270).

---

## 1. Criação de Tarefa
Cria uma nova tarefa. O sistema define automaticamente o status inicial como `Pending` e gera as datas de criação e atualização. O título é obrigatório.

**Requisição:**
```bash
curl -X POST http://localhost:5000/api/task \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Configurar ambiente de produção",
    "description": "Instalar Docker e provisionar banco de dados PostgreSQL."
  }'
```

---

## 2. Consulta de Tarefas (Paginada)
Retorna a lista de tarefas de forma paginada. Permite filtrar por status.

**Parâmetros de Query:**
* `pageNumber` (padrão: 1)
* `pageSize` (padrão: 10, máximo: 50)
* `status` (opcional. 0 = Pending, 1 = InProgress, 2 = Done)

**Requisição (Página 1, 10 itens):**
```bash
curl -X GET "http://localhost:5000/api/task?pageNumber=1&pageSize=10" \
  -H "Accept: application/json"
```

**Requisição (Filtrando por status 'Pending' - 0):**
```bash
curl -X GET "http://localhost:5000/api/task?pageNumber=1&pageSize=10&status=0" \
  -H "Accept: application/json"
```

---

## 3. Consulta de Tarefa por ID
Retorna os dados completos de uma tarefa específica utilizando o seu identificador único (UUID).

**Requisição:**
*(Substitua `{id}` pelo UUID retornado na criação da tarefa)*
```bash
curl -X GET http://localhost:5000/api/task/{id} \
  -H "Accept: application/json"
```

---

## 4. Atualização de Detalhes da Tarefa
Atualiza o título e/ou a descrição de uma tarefa existente.

**Requisição:**
```bash
curl -X PUT http://localhost:5000/api/task/{id}/details \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Configurar ambiente de produção (Atualizado)",
    "description": "Instalar Docker, provisionar banco de dados e configurar CI/CD."
  }'
```

---

## 5. Atualização de Status da Tarefa
Avança o status da tarefa. O fluxo obrigatório é: **Pending (0) → InProgress (1) → Done (2)**. Tentativas de saltar etapas ou reverter o status retornarão erro `409 Conflict`.

**Requisição (Avançando para InProgress):**
```bash
curl -X PATCH http://localhost:5000/api/task/{id}/status \
  -H "Content-Type: application/json" \
  -d '{
    "status": 1
  }'
```

---

## 6. Remoção de Tarefa
Remove uma tarefa existente do banco de dados.

**Requisição:**
```bash
curl -X DELETE http://localhost:5000/api/task/{id} \
  -H "Accept: application/json"
```