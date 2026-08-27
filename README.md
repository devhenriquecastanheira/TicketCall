<!-- generated-by: gsd-doc-writer -->
# TicketCall

API REST para gerenciar chamados de suporte, consultar tickets, alterar status e acompanhar o historico de mudancas de status.

## Objetivo

O TicketCall centraliza o ciclo de vida de tickets de atendimento. A API permite criar, listar, filtrar, atualizar e excluir tickets, alem de registrar historico sempre que o status muda por uma transicao valida.

## Stack

- .NET 8 (`net8.0`)
- ASP.NET Core Web API com controllers
- Entity Framework Core 8
- PostgreSQL via `Npgsql.EntityFrameworkCore.PostgreSQL`
- Swagger/OpenAPI via `Swashbuckle.AspNetCore`

## Estrutura

```text
TicketCall.slnx
TicketCall.Api/
  Controllers/TicketsController.cs
  Data/AppDbContext.cs
  Dtos/CreateTicketDto.cs
  Entities/
  Migrations/
  Program.cs
```

## Como Rodar

1. Configure um PostgreSQL local e crie o banco `ticketcall`.
2. Ajuste a connection string, se necessario, em `TicketCall.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=ticketcall;Username=postgres"
}
```

3. Restaure os pacotes:

```bash
dotnet restore
```

4. Aplique as migrations:

```bash
dotnet ef database update --project TicketCall.Api
```

5. Inicie a API:

```bash
dotnet run --project TicketCall.Api
```

Em desenvolvimento, os perfis de launch expõem a API em `http://localhost:5253` e `https://localhost:7225`. O Swagger fica disponivel em `/swagger` quando `ASPNETCORE_ENVIRONMENT=Development`.

## Endpoints

Base local padrao: `http://localhost:5253`.

| Metodo | Caminho | Descricao |
|---|---|---|
| `GET` | `/api/tickets` | Lista tickets. Aceita filtros opcionais por `status`, `priority` e `search`. |
| `GET` | `/api/tickets/{id}` | Busca um ticket por ID. Retorna `404` quando nao encontrado. |
| `POST` | `/api/tickets` | Cria um ticket. Retorna `201 Created` com o ticket criado. |
| `PUT` | `/api/tickets/{id}` | Atualiza titulo, descricao e prioridade de um ticket. Retorna `404` quando nao encontrado. |
| `DELETE` | `/api/tickets/{id}` | Remove um ticket. Retorna `404` quando nao encontrado. |
| `PATCH` | `/api/tickets/{id}/status?newStatus={status}` | Altera o status do ticket e registra historico quando a transicao e valida. |
| `GET` | `/api/tickets/{id}/history` | Lista o historico de mudancas de status do ticket. Retorna `404` quando o ticket nao existe. |

### Filtros de Listagem

`GET /api/tickets` aceita:

- `status`: `Open`, `InProgress`, `Resolved` ou `Cancelled`
- `priority`: `Low`, `Medium`, `High` ou `Critical`
- `search`: texto procurado em `Title` ou `Description`

Exemplo:

```bash
curl "http://localhost:5253/api/tickets?status=Open&priority=High&search=login"
```

### Criar Ticket

```bash
curl -X POST "http://localhost:5253/api/tickets" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Erro no login",
    "description": "Usuario nao consegue autenticar",
    "status": "Open",
    "priority": "High"
  }'
```

Corpo aceito pelo `CreateTicketDto`:

```json
{
  "title": "Erro no login",
  "description": "Usuario nao consegue autenticar",
  "status": "Open",
  "priority": "High"
}
```

### Atualizar Ticket

```bash
curl -X PUT "http://localhost:5253/api/tickets/1" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Erro no login",
    "description": "Falha ao autenticar com credenciais validas",
    "priority": "Critical"
  }'
```

### Alterar Status

Transicoes implementadas:

- `Open` -> `InProgress`
- `Open` -> `Cancelled`
- `InProgress` -> `Resolved`
- `InProgress` -> `Cancelled`

Exemplo:

```bash
curl -X PATCH "http://localhost:5253/api/tickets/1/status?newStatus=InProgress"
```

Se o novo status for igual ao atual ou a transicao nao for permitida, a API retorna `400 Bad Request`.

### Consultar Historico

```bash
curl "http://localhost:5253/api/tickets/1/history"
```

O retorno contem registros com `ticketId`, `oldStatus`, `newStatus` e `changedAt`.

## Configuracao

A connection string principal se chama `DefaultConnection` e e lida em `Program.cs` por `builder.Configuration.GetConnectionString("DefaultConnection")`.

Por padrao no repositorio:

```json
"DefaultConnection": "Host=localhost;Port=5432;Database=ticketcall;Username=postgres"
```

Se o PostgreSQL exigir senha, inclua `Password=...` na connection string local ou use User Secrets para nao versionar credenciais.
