
# Base-exchange

Aplicação desenvolvida para calcular a **exposição financeira por ativo**, com base em ordens de compra e venda.

---

## 🚀 Stacks Utilizadas

- ASP.NET Core 8  
- Entity Framework Core  
- SQL Server  
- Docker  
- ReactJS (a ser implementado)

---

## 🛠️ Banco de Dados & ORM

O projeto utiliza **Entity Framework Core com SQL Server** para persistência de dados.

Em projetos reais costumo escrever as queries à mão, mas utilizei `Migrations` para facilitar e estruturar o desafio técnico.

### 📦 Pacotes utilizados

```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet tool install --global dotnet-ef
```

### 🏗️ Criação do banco e tabelas

As tabelas são criadas automaticamente a partir das `Migrations`.

```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

✅ **Importante**: ao rodar a aplicação, o EF Core aplica automaticamente as migrations (via `Database.Migrate()`), não sendo necessário criar o banco manualmente.

---

### 🐞 Desafio encontrado

Ao utilizar `HasData()` no `OnModelCreating`, usei `DateTime.UtcNow`, o que fazia com que o EF gerasse novas migrations a cada build.  
Corrigi isso fixando a data com `new DateTime(2024, 04, 18)` conforme a recomendação da documentação oficial.

---

## 🧱 Arquitetura em Camadas

### 📁 Camada Domain

Contém os modelos de domínio:

- `ExposicaoFinanceira`
- `Ordem`

Seguindo princípios do DDD, essa camada se mantém isolada.

---

### 📁 Camada Infra

Contém o `DBContext` e a `DbContextFactory`.

> A `DbContextFactory` foi necessária para que fosse possível rodar as migrations via CLI (`dotnet ef`) em um projeto separado da API.

---

### 📁 Camada App

Contém:

- Service: `OrdemService`
- Interface: `IOrdemService`
- DTOs: `OrderRequestDTO` e `OrderResponseDTO`
- Validações: `OrderRequestValidator` (FluentValidation)

> Inicialmente pensei em criar uma camada `Contract` para os DTOs, mas como eles só são usados entre App e API, optei por mantê-los no `App`.

---

### ✅ FluentValidation

As validações são feitas no DTO (`OrderRequestDTO`) utilizando FluentValidation de forma desacoplada da Controller:

```csharp
public class OrderRequestValidator : AbstractValidator<OrderRequestDTO> { ... }
```

---

### 🆕 Recurso: Primary Constructors

Aproveitei para utilizar um recurso novo do C# 12, os **Primary Constructors**, eliminando a necessidade de declarar construtores manuais em serviços como:

```csharp
public class OrdemService(DBContext context) : IOrdemService { ... }
```

---

## 🧪 Testes Automatizados

Foi criado o projeto de testes `Testes` com:

- **xUnit** para testes unitários
- **EF Core InMemory** simulando o banco de dados
- **Moq** para simulações (quando necessário)

### 🧬 Casos testados

- Ordem válida
- Ordem que ultrapassa a exposição
- Ordem de venda
- Ativo inexistente

### 📦 Pacotes utilizados

```
dotnet add package xunit
dotnet add package Moq
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

### ▶️ Executando os testes

```
cd Testes
dotnet test
```

---

## ⚙️ Como Executar Localmente

### 🧭 Clone o repositório

```
git clone https://github.com/themanuca/Base-exchange.git
cd Base-exchange
```

### 🧰 Configure o banco de dados local (SQL Server)

No `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=BaseExchangeDb;User Id=sa;Password=SenhaForte123!;Encrypt=true;TrustServerCertificate=true;"
}
```

> O banco será criado automaticamente na primeira execução da aplicação.

### 📦 Restaure os pacotes

```
dotnet restore
```

### 🛠️ Execute a aplicação

```
dotnet run --project APIBaseExchange
```

### 🌐 Acesse o Swagger

```
http://localhost:5000/swagger
```

---

## 🐳 Executando com Docker

O projeto possui um `docker-compose.yml` que sobe a **API, SQL Server, e o Front** automaticamente.

### ▶️ Subir os containers

```
docker-compose up --build
```

### 🌐 Acessar o Swagger da API

```
http://localhost:8080/swagger
```
### 🧠 Observação importante

A API executa automaticamente o `Database.Migrate()` na inicialização.  
✅ Isso significa que **o banco é criado automaticamente, sem necessidade de rodar scripts manuais**.

---

### 🧭 Connection String usada no Docker

```json
"Server=host.docker.internal,1433;Database=BaseExchangeDb;User=sa;Password=SenhaForte123!;TrustServerCertificate=True;"
```

> Se preferir rodar a API com banco local, basta comentar o serviço `sqlserver` no `docker-compose.yml`


## 🖥️ Frontend React

O projeto possui uma interface em React + TypeScript desenvolvida com Vite.

Ela permite:

- Enviar ordens de compra e venda
- Visualizar a exposição financeira resultante
- Validação dos dados do formulário
- Feedback visual de sucesso e erro

### ▶️ Executando localmente

```bash
cd BaseExchangeFront
npm install
npm run dev
