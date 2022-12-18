
TelephoneDirectory is a microservice-based application for managing a directory of telephone numbers. It consists of two microservices: contract and report, and a consumer. The contract microservice is responsible for managing the telephone directory data and exposes a REST API for creating, reading, updating, and deleting entries in the directory. The report microservice is responsible for generating reports based on the directory data and also exposes a REST API for requesting reports. The consumer is a background worker that listens for messages on a message queue and processes them. In this case, it listens for requests for reports and generates them using the report microservice.

This application is built using ASP.NET Core and can be easily deployed using Docker and Docker Compose. It also includes a database and message queue for storing and processing data.

## Features

-   Create, read, update, and delete entries in the telephone directory
-   Generate reports based on the directory data
-   Asynchronous processing of report requests using a message queue
-   Deployable as a set of Docker containers

## Prerequisites

-   Docker and Docker Compose

## Running the Application

To start the application, execute the following command in the root directory of the project:

`docker-compose -f docker-compose-prod.yml up --build -d` 

You can then access the following URLs:

-   Contract API: [http://localhost:8080](http://localhost:8080/)
-   Report API: [http://localhost:8081](http://localhost:8081/)
-   Adminer (database GUI): [http://localhost:8282](http://localhost:8282/)
-   RabbitMQ GUI: [http://localhost:15672](http://localhost:15672/)

## Credentials

-   RabbitMQ GUI: username: `guest`, password: `guest`
-   Adminer (database GUI): server: postgres username: `phonedirectory_usr`, password: `PZLqwVFf8YkwqRhq?PZLqwVFf8Y_prod`, database name: `phonedirectory_db`

## Development

To run the application in development mode, execute the following command in the root directory of the project:


`docker-compose -f docker-compose-dev.yml up --build` 

This will start the microservices and their dependencies with hot reloading enabled.
