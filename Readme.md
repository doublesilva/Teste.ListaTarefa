# Lista Tarefas API

Este projeto é uma API .NET 8 utilizando Entity Framework Core com SQLite e Docker, seguindo os princípios de Domain-Driven Design (DDD) e CQRS.

## Arquitetura

A solução é composta por quatro projetos principais:

1. **Domain**: Contém as entidades do domínio, interfaces e lógica de negócios.
2. **Infrastructure**: Implementação das interfaces, acesso a dados e configuração do EF Core.
3. **Application**: Contém os casos de uso, DTOs, comandos e consultas utilizando CQRS com MediatR.
4. **WebApi**: Camada de apresentação com controladores e configuração do Docker.

### Diagrama da Arquitetura

```plaintext
+----------------+          +-----------------+
|                |          |                 |
|     WebApi     +<-------->+   Application   |
|                |          |                 |
+--------+-------+          +--------+--------+
         ^                           ^
         |                           |
         v                           v
+--------+-------+          +--------+--------+
|                |          |                 |
| Infrastructure +<-------->+      Domain     |
|                |          |                 |
+----------------+          +-----------------+
```

## Configuração e Execução

### Pré-requisitos

Certifique-se de ter o Docker e o Docker Compose instalados na sua máquina.

### Passos para Rodar o Projeto

1. **Clone o Repositório**

   ```bash
   git clone https://github.com/doublesilva/Teste.ListaTarefa.git
   cd Teste.ListaTarefa
   ```

2. **Build e Run com Docker Compose**

   Execute o seguinte comando para compilar e iniciar os serviços definidos no `docker-compose.yml`:

   ```bash
   docker-compose up --build
   ```

   Isso irá iniciar a aplicação na porta `5000` e criar o banco de dados SQLite no diretório `./AppData`.

3. **Acessar a Documentação Swagger**

   Após iniciar a aplicação, acesse a documentação Swagger no seguinte URL:

   ```url
   http://localhost:5000/swagger/index.html
   ```

## Estrutura dos Diretórios

A estrutura do projeto está organizada da seguinte forma:

```plaintext
Teste.ListaTarefa
├── Teste.ListaTarefa.Domain
│   ├── Entities
│   ├── Interfaces
│   └── Services
├── Teste.ListaTarefa.Infrastructure
│   ├── Repositories
│   └── Configuration
├── Teste.ListaTarefa.Application
│   ├── Commands
│   ├── Queries
│   ├── Handlers
│   └── DTOs
├── Teste.ListaTarefa.WebApi
│   ├── Controllers
│   ├── Middleware
│   └── Docker
└── Tests
    ├── Teste.ListaTarefa.UnitTest
    └── Teste.ListaTarefa.IntegrationTest
```

### Executando os Testes

#### Testes Unitários

Para executar os testes unitários, utilize o comando:

```bash
dotnet test Teste.ListaTarefa.UnitTest/Teste.ListaTarefa.UnitTest.csproj
```

#### Testes de Integração

Para executar os testes de integração, utilize o comando:

```bash
dotnet test Teste.ListaTarefa.IntegrationTest/Teste.ListaTarefa.IntegrationTest.csproj
```