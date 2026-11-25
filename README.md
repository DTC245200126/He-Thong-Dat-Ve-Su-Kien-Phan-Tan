
# HỆ THỐNG ĐẶT VÉ SỰ KIỆN PHÂN TÁN

## 1. Giới thiệu

Đồ án học phần Phát triển ứng dụng phân tán.

Đề tài 03: Hệ thống đặt vé sự kiện phân tán.

Hệ thống cho phép người dùng xem danh sách sự kiện,
xem chi tiết sự kiện, gửi yêu cầu đặt vé và theo dõi
trạng thái đặt vé.

## 2. Công nghệ sử dụng

- Ngôn ngữ: C#
- Framework: .NET 10
- Backend: ASP.NET Core Minimal API
- Database: SQL Server
- ORM: Entity Framework Core
- Client: Console Application
- Message Broker dự kiến: RabbitMQ
- Background Service dự kiến: Booking Worker
- Công cụ: Visual Studio Code, Git, GitHub, SSMS

## 3. Cấu trúc dự án

He-Thong-Dat-Ve-Su-Kien-Phan-Tan/
├── TicketSystem.slnx
├── src/
│   ├── Ticket.Api/
│   │   ├── Data/
│   │   ├── DTOs/
│   │   ├── Models/
│   │   ├── Migrations/
│   │   └── Program.cs
│   ├── Booking.Worker/
│   └── Ticket.Client/
└── README.md

## 4. Kiến trúc hệ thống dự kiến

Ticket Client
    |
    v
Ticket API
    |
    v
RabbitMQ
    |
    v
Booking Worker
    |
    v
SQL Server Database

Ticket API cũng kết nối trực tiếp với Database để
truy vấn sự kiện và lưu yêu cầu đặt vé.

Lưu ý: RabbitMQ và xử lý đặt vé bằng Booking Worker
chưa được tích hợp ở giai đoạn hiện tại.

## 5. Các chức năng đã thực hiện

### 5.1. Event API

- GET /events: Lấy danh sách sự kiện.
- GET /events/{id}: Xem chi tiết sự kiện.

### 5.2. Booking API

- POST /bookings: Tạo yêu cầu đặt vé.
- GET /bookings/{id}: Xem chi tiết đơn đặt vé.
- GET /users/{userId}/bookings: Xem lịch sử đặt vé.

Đơn đặt vé mới được lưu với trạng thái Pending.

Việc xác nhận hoặc từ chối đặt vé sẽ được xử lý
ở giai đoạn tích hợp RabbitMQ và Booking Worker.

### 5.3. Database

Sử dụng SQL Server với database TicketBookingDB.

Các bảng chính:
- Events
- Users
- Bookings

Sử dụng Entity Framework Core Code First
và Migrations để quản lý cấu trúc dữ liệu.

## 6. Hướng dẫn chạy dự án

Yêu cầu:
- .NET 10 SDK
- SQL Server
- Database TicketBookingDB

Cập nhật ConnectionStrings:DefaultConnection
trong src/Ticket.Api/appsettings.json cho phù hợp
với SQL Server trên máy đang chạy.

Áp dụng EF Core Migrations nếu database chưa
được khởi tạo.

Build solution:

    dotnet build

Chạy Ticket API:

    dotnet run --project src/Ticket.Api --urls http://localhost:5100

Các địa chỉ kiểm tra:

- http://localhost:5100
- http://localhost:5100/db-check
- http://localhost:5100/events
- http://localhost:5100/events/1
- http://localhost:5100/bookings/1
- http://localhost:5100/users/1/bookings

Để kiểm thử POST /bookings, cần có dữ liệu
Events và Users trong database.

## 7. Tiến độ thực hiện

### Tuần 1 - Khởi tạo dự án

- Tạo solution .NET 10.
- Tạo Ticket.Api, Ticket.Client và Booking.Worker.
- Khởi tạo Git và GitHub repository.

### Tuần 2 - Database và Event API

- Kết nối SQL Server.
- Tạo các Models Event, User, Booking.
- Tạo AppDbContext và EF Core Migrations.
- Xây dựng GET /events.
- Xây dựng GET /events/{id}.
- Kiểm thử kết nối Database.

### Tuần 3 - Booking API

- Tạo CreateBookingRequest DTO.
- Xây dựng POST /bookings.
- Kiểm tra dữ liệu đầu vào.
- Lưu Booking với trạng thái Pending.
- Xây dựng GET /bookings/{id}.
- Xây dựng GET /users/{userId}/bookings.
- Kiểm thử tạo và truy vấn đơn đặt vé.

### Các giai đoạn tiếp theo

- Tích hợp RabbitMQ.
- Xây dựng xử lý nghiệp vụ trong Booking Worker.
- Cập nhật trạng thái Confirmed hoặc Rejected.
- Kiểm soát số lượng vé còn lại.
- Xây dựng Console Client gọi API.
- Docker hóa và kiểm thử hệ thống phân tán.

## 8. Quản lý mã nguồn

Sử dụng GitHub để quản lý mã nguồn.

Các nhánh chức năng:
- main
- feature/database-events
- feature/booking

Mỗi chức năng được phát triển trên nhánh riêng,
sau đó tạo Pull Request để hợp nhất vào main.
