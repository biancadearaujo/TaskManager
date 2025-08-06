# TaskManager Backend

Este é o backend da aplicação TaskManager, uma API robusta e escalável desenvolvida em **.NET** para gerenciar usuários e suas tarefas. O projeto foi arquitetado para lidar com toda a lógica de negócio, persistência de dados e segurança, fornecendo uma base sólida para qualquer frontend.

## Funcionalidades

- **Arquitetura Limpa e DDD:** O código é estruturado seguindo os princípios de Domain-Driven Design (DDD), o que resulta em um sistema organizado, fácil de testar e de alta escalabilidade.
- **Autenticação com JWT:** Sistema de segurança completo com endpoints para registro e login de usuários, utilizando **tokens JWT** para autenticar e proteger o acesso às rotas da API.
- **Gerenciamento Seguro de Tarefas (CRUD):** As operações de Criar, Ler, Atualizar e Deletar tarefas garantem que cada usuário possa acessar e modificar apenas as suas próprias tarefas, mantendo a integridade e privacidade dos dados.
- **Persistência de Dados Híbrida:** Acesso a dados otimizado combinando o **Entity Framework Core** para gerenciar as migrações do esquema do banco de dados e o **Dapper**, um micro-ORM de alta performance, para operações CRUD nas tarefas.
- **Ambiente Dockerizado:** Todas as dependências da aplicação (PostgreSQL) são orquestradas com **Docker Compose**, garantindo um ambiente de desenvolvimento e produção consistente e isolado.

---

## Tecnologias Utilizadas

- **[.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)** - Plataforma de desenvolvimento moderna e de alta performance.
- **[ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet)** - Framework para a construção da API web.
- **[Domain-Driven Design (DDD)](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)** - Abordagem arquitetónica para sistemas complexos.
- **[Dapper](https://www.learndapper.com/)** - Micro-ORM de alta performance para acesso a dados.
- **[Entity Framework Core](https://learn.microsoft.com/pt-br/ef/core/)** - Usado para gerir as migrações do esquema da base de dados.
- **[PostgreSQL](https://www.postgresql.org/)** - Sistema de gestão de base de dados relacional.
- **[JWT (JSON Web Tokens)](https://www.jwt.io/introduction)** - Padrão para a criação de tokens de acesso.
- **[xUnit](https://xunit.net/?tabs=cs)** - Framework para testes unitários e de integração.
- **[Moq](https://github.com/moq/moq4)** - Biblioteca de mocking para testes unitários.
- **[FluentAssertions](https://fluentassertions.com/)** - Biblioteca para facilitar a escrita de asserções nos testes.
- **[Docker](https://www.docker.com/)** - Para a contentorização e orquestração da aplicação.

---

## Como Executar o Projeto

A maneira mais fácil e recomendada de executar o projeto é usando **Docker Compose**, que irá configurar e iniciar o banco de dados e a API automaticamente.

### Pré-requisitos

- [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/)

### Instalação

1. Clone o repositório para a sua máquina:
   ```bash
   git clone https://github.com/biancadearaujo/TaskManager.git
   ```

```
cd TaskManager
docker-compose up --build
```

```
dotnet test ./tests/TaskManager.Tests.Unit/
```

```
dotnet test ./tests/TaskManager.Tests.Integration/
```


