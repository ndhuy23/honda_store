Architecture: Microservices

Technologies: Json Web Token (JWT), .NET, MSSQL, MongoDB, Masstransit, RabbitMQ, gRPC,..

Description: The Honda Store project adopts microservice architecture. I use MassTransit in conjunction with RabbitMQ for inter-service communication and ensure optimal database performance. I utilize gRPC for communication between the Product Service and Order Service to check product quantity of product in Product Service. JWT is utilized for authentication and authorization in API Gateway with Ocelot in conjunction with User Service. 

System Architecture Design:
![image](https://github.com/ndhuy23/honda_store/assets/116952849/35ac2e20-ad90-422a-b2e3-8a51802aec66)
