# finance-manager-api

It's a simple REST API built in learning purposes.

## Description

Finance manager API stores data of transactions for users.
There are 12 predefined transaction categories (can be changed by modifying TransactionCategoryId Enum).
To access API resources you need to authemticate as a user. API uses JWT for authentication.
Swagger is configurated for API and can be used as documentation and endpoint testing.

## Libraries

* AutoMapper
* Swashbuckle
* Identity
* Entity Framework Core

## How to run

Use docker and run commands in solution folder:
```
docker-compose build
docker-compose up
```
Swagger documentation will be available at
https://localhost:5000/swagger/index.html
