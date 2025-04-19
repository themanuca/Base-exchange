# Base-exchange
Aplicação Deve calcular a Exposição Financeira por ativo.

## Stacks
- ASP.NET CORE 8;
- ENTITY FRAMEWORK;
- SQL SERVER;
- DOCKER;
- REACTJS

### Banco de Dados & ORM
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

## ⚙️  Design-time DbContext Factory 
Foi criado um DBContextFactory para caso precisem execultar a migration sem execultar a aplicação.
Devido a separação dos projetos em camadas (Infra, Domain, App, API), foi necessário criar uma DBContextFactory que buscasse a configuração da API para permitir a geração de migrations com ```dotnet ef```.

Hedando o ```IDesignTimeDbContextFactory<ApplicationDbContext>``` para permitir a execução correta dos comandos de migrations via CLI (dotnet ef migrations add, dotnet ef database update), já que o EF Core não consegue resolver a injeção de dependência automaticamente fora da aplicação ASP.NET.

A classe utiliza ConfigurationBuilder apontando para o caminho do appsettings.json, garantindo que a connection string seja lida de forma centralizada.

### Docker
Estou utlizando um container do SQL Server.

