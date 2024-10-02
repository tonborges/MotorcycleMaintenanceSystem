# Motocycle Maintenance System

## Content

- [About the Project](#about-the-project)
- [Services](#services)
- [Functional Tests](#functional-tests)
- [Integration Tests](#integration-tests)
- [Unit Tests](#unit-tests)
- [EndPoints](#endPoints)
- [Enums](#enumeradores)
- [Database](#database)
- [Technologies and Frameworks](#technologies-and-frameworks)

---

## About the Project

This project was developed as a practical study for creating a motorcycle maintenance application. The application was built using .NET Core 8, following the clean architecture pattern and incorporating the principles of CQRS (Command Query Responsibility Segregation), SOLID, and event-driven architecture, aiming for a robust and highly scalable structure.

---

## Services

- [Motocycle Maintenance System - API](http://localhost:8080)
  - http://localhost:8080
- [Seq - Log management service](http://localhost:8081)
  - http://localhost:8081
- [minIO - Object storage service](http://localhost:9001)
  - http://localhost:9001
      - username: minioadmin
      - password: minioadmin
- [RabbitMQ - Message broker service](http://localhost:15672)
  - http://localhost:15672
    - username: guest
    - password: guest
---

## Functional Tests

The functional tests were developed using the xUnit framework. The tests are located in the `tests` folder and can be run using the following instructions:

start the docker-compose services:

```bash
docker-compose build
docker-compose up
```

run the tests:

```bash
dotnet test tests/FunctionalTests/FunctionalTests.csproj
```
---

## Integration Tests

The integration tests were developed using the xUnit framework and mocks. The tests are located in the `tests` folder and can be run using the following instructions:

```bash
dotnet test tests/FunctionalTests/FunctionalTests.csproj
```
---

## Unit Tests

The unit tests were developed using the xUnit framework. The tests are located in the `tests` folder and can be run using the following instructions:

```bash
dotnet test tests/UnitTests/UnitTests.csproj
```

---
## EndPoints

| HTTP Verb | EndPoint                | Description 							|
|-----------|-------------------------|--------------------------------------|
| POST      | /entregadores           | Register delivery person                                    
| POST      | /entregadores/{id}/cnh  | Search existing motorcycles                                 
| GET       | /motos                  | Cadastra a solicitação de relatório na fila de processamento 
| POST      | /motos                  | Register a new motorcycle                                   
| GET       | /motos/{id}             | Search for a motorcycle by Id
| PUT       | /motos/{id}/placa       | Modify a motorcycle's plate
| DELETE    | /motos/{id}             | Remove a motorcycle
| GET       | /locacao/{id}           | Search for a rentals by Id
| POST      | /locacao                | Register a new rental
| PUT       | /locacao/{id}/devolucao | Enter return date and calculate value
---

## Enums

| Enum        | Description                                        |
|-------------|----------------------------------------------------|
| DriversLicenseType | Type of driver's license (A, B, AB, C, D, E) |

---

## Database

Entityframework context connecting to the database **postgres**.

```json
{
  "ConnectionStrings": {
    "Database": "Host=postgres;Port=5432;Database=postgres;Username=<USERNAME>;Password=<PASSWORD>Include Error Detail=true"
  }
}
```

---

## Technologies and Frameworks

- .NET Core 8;
- Serilog;
- Seq;
- Xunit;
- Mock;
- Postgres;
- RabbitMQ;
- minIO;
- Docker;
- FluentValidation;
- FluentAssertions;