
# Hệ thống đặt vé sự kiện phân tán

## 1. Giới thiệu
Bài tập lớn học phần Phát triển ứng dụng phân tán.

Đề tài 03: Hệ thống đặt vé sự kiện phân tán.

Hệ thống cho phép người dùng xem sự kiện, gửi yêu cầu đặt vé và theo dõi trạng thái xử lý. Việc xác nhận vé được xử lý bất đồng bộ thông qua RabbitMQ và Booking Worker.

## 2. Công nghệ sử dụng
- C# / .NET 10
- ASP.NET Core Minimal API
- Console Application
- RabbitMQ (dự kiến tích hợp)
- Database (sẽ lựa chọn ở giai đoạn tiếp theo)
- Docker và Docker Compose (dự kiến triển khai)

## 3. Cấu trúc dự án
- src/Ticket.Api: API quản lý sự kiện và đặt vé.
- src/Booking.Worker: Service xử lý đặt vé bất đồng bộ.
- src/Ticket.Client: Ứng dụng Console giao tiếp với API.

## 4. Hướng dẫn build
Yêu cầu: .NET 10 SDK.

Chạy lệnh:

```powershell
dotnet restore
dotnet build
```

## 5. Tiến độ
- [x] Khởi tạo solution .NET 10.
- [x] Tạo Ticket.Api.
- [x] Tạo Booking.Worker.
- [x] Tạo Ticket.Client.
- [x] Build thành công.
- [ ] Xây dựng Database và API sự kiện.
- [ ] Xây dựng API đặt vé.
- [ ] Tích hợp RabbitMQ và Booking Worker.
- [ ] Hoàn thiện Docker Compose.
- [ ] Kiểm thử và hoàn thiện báo cáo.
