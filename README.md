# Base-exchange
Aplicação Deve calcular a Exposição Financeira por ativo.

## Stacks
- ASP.NET CORE 8;
- ENTITY FRAMEWORK;
- SQL SERVER;
- DOCKER;
- REACTJS

### 🛠️ Banco de Dados & ORM
 O projeto utiliza Entity Framework Core com SQL Server para persistência dos dados.
 
 Geralmente monto a query na mão, mas o "migration" facilita muito esse processo.
 
 **Instalação**:
 - ```dotnet add package Microsoft.EntityFrameworkCore.SqlServer```
 - ```dotnet add package Microsoft.EntityFrameworkCore.Tools```
 - ```dotnet tool install --global dotnet-ef```
 
 **Migrations**:
 As tabelas são criadas automaticamente via ```dotnet ef database update```, com base nas migrations.
 Para realização da migrations precisou-se somente rodar os comandos:
 - ```dotnet ef migrations add InitialCreate```
 - ```dotnet ef database update```
 
 Desafio encontrado
 
 Durante a criação do seed com HasData(), utilizei DateTime.UtcNow, o que gerava migrações "pendentes" a cada build, pois o valor era dinâmico. O problema foi resolvido substituindo por uma data fixa (new DateTime(2024, 04, 18)), conforme orientação da documentação oficial.
 
 Criação da base: ```CREATE DATABASE BaseExchangedb```
 
 Tabelas criadas:
 
 - Ordens: registra todas as ordens recebidas, aceitas ou rejeitadas.
 
 - ExposicoesFinanceiras: armazena a exposição financeira atual de cada ativo.
 
  O SQL Server ta rodando em um container, comando de criação da base.

### ⚙️  Design-time DbContext Factory 
 Foi criado um DBContextFactory para caso precisem execultar a migration sem execultar a aplicação.
 Devido a separação dos projetos em camadas (Infra, Domain, App, API), foi necessário criar uma DBContextFactory que buscasse a configuração da API para permitir a geração de migrations com ```dotnet ef```.
 
 Hedando o ```IDesignTimeDbContextFactory<DBContext>``` para permitir a execução correta dos comandos de migrations via CLI (dotnet ef migrations add, dotnet ef database update), já que o EF Core não    consegue resolver a injeção de dependência automaticamente fora da aplicação ASP.NET.
  A classe utiliza ConfigurationBuilder apontando para o caminho do appsettings.json, garantindo que a connection string seja lida de forma centralizada.
## 🧱 Camada App
 Foi criado a service, sua interface e os DTOs. Inicialmente tinha pensado em criar uma camada Contract, para os DTOs, mas como o projeto é pequeno e os DTOs só serão usados no APP e API, optei em manter no mesmo.
 
 Services:
 - OrdemServices
   
 Interfaces:
 - IOrdemServices
   
 DTOs:
 - OrderResponseDTO
 - OrderRequestDTO

 ### FluentValidation - DTOs
  Foi adicionado validação no DTO antes de chegar na controller. Evitando validações na controllers ou service e garantindo uma validação estruturada e desacoplada.
   ```OrderRequestValidator```, herda o ```AbstractValidator<OrderRequestDTO>```.
  
 App depende da camada ```Infra```, por causa do **DBContext**, que é chamado no construtor.

## 🧱 Camda Domain
Na camada de dominio foi criado as entidades/modelos, ```ExposicaoFinaceira``` e ```Ordem```. 
Respeitando os principios do DDD, a dominio se mantem isolada.

## 🧱 Camada Infra
Na Infra temos a conexão com o banco e a migration. Caso haja a necessidade da migração sem rodar o projeto foi criado um factory para instaciar o dbocontext sem ta em runtime.
```DBContext , DBContextFactory```.
## Primary Constructors
Aproveitei para usar um recurso novo, que veio junto com o C# 12, chamado "Primary Constructors".

Com o novo recuso abrstraio a necessidade de criar um construtor, passando o parametro no corpo da classe.

## 🧪 Testes Automatizados
Criado um projeto de Teste xUnit, ```Testes```.

- Testes unitários implementados com xUnit
 
- Banco em memória para simular comportamento real
- Casos testados:git clone https://github.com/themanuca/Base-exchange.git
cd Base-exchange

  - Ordem válida
  - Exposição acima do limite
  - Ordem de venda
  - Ativo inválido
    
Libs instaladas:
- ```Moq```
- ```EntityFrameworkCore.InMemory```
  
Foi adicionado a referencia da camada do App ```App.csproj```.

## 🚀 Como Executar o Projeto
**Clone o projeto**
```
git clone https://github.com/themanuca/Base-exchange.git
cd Base-exchange
```
**Configure o banco de dados (SQL Server)**
```
 "ConnectionStrings": {
   "DefaultConnection": "Server=localhost;Database=BaseExchangedb;Encrypt=true;TrustServerCertificate=true;"
 },
```

### 🐳 Docker
Estou utlizando um container do SQL Server.

