Kiến trúc: Microservice

Công nghệ: Json Web Token (JWT), .NET, MSSQL, MongoDB, Masstransit, RabbitMQ, gRPC,..

Mô tả: Honda Store là một trang web bán xe sử dụng kiến trúc microservice. Sử dụng RabbitMQ để làm Event bus liên lạc giữa các service và sử dụng gRPC để thực hiện các request-response giữa các service cần thực hiện điều đó. Sử dụng Ocelot kết hợp với User Service để thực hiện xác thực và phân quyền ngay tại API Gateway. Dự án có khả năng mở rộng trong tương lai.
