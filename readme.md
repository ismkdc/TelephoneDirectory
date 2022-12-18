
# TelephoneDirectory

TelephoneDirectory is a microservice-based application for managing a directory of telephone numbers. It consists of two microservices: contract and report, and a consumer.

## Prerequisites

-   Docker and Docker Compose

## Running the Application

To start the application, execute the following command in the root directory of the project:

Copy code

`docker-compose -f docker-compose-prod.yml up --build -d` 

This will build and start the three microservices and their dependencies (database and message queue) in Docker containers.

You can then access the following URLs:

-   Contract API: [http://localhost:8080](http://localhost:8080/)
-   Report API: [http://localhost:8081](http://localhost:8081/)
-   Adminer (database GUI): [http://localhost:8282](http://localhost:8282/)
-   RabbitMQ GUI: [http://localhost:15672](http://localhost:15672/)

## Credentials

-   RabbitMQ: username: `guest`, password: `guest`
-   Database: username: `phonedirectory_usr`, password: `PZLqwVFf8YkwqRhq?PZLqwVFf8Y_prod`, database name: `phonedirectory_db`

## Development

To run the application in development mode, execute the following command in the root directory of the project:

Copy code

`docker-compose -f docker-compose-dev.yml up --build` 

This will start the microservices and their dependencies with hot reloading enabled.

## Notes

-   The `contract` microservice is responsible for managing the telephone directory data. It exposes a REST API for creating, reading, updating, and deleting entries in the directory.
-   The `report` microservice is responsible for generating reports based on the directory data. It also exposes a REST API for requesting reports.
-   The `consumer` is a background worker that listens for messages on a message queue and processes them. In this case, it listens for requests for reports and generates them using the `report` microservice.