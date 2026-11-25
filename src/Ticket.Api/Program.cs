
using Microsoft.EntityFrameworkCore;
using Ticket.Api.Data;
using Ticket.Api.DTOs;
using Ticket.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Đọc chuỗi kết nối SQL Server
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

// Đăng ký Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// API kiểm tra hoạt động
app.MapGet("/", () => "Ticket API is running");

// API kiểm tra kết nối Database
app.MapGet("/db-check", async (AppDbContext db) =>
{
    var connected = await db.Database.CanConnectAsync();

    return connected
        ? Results.Ok("Database connected successfully")
        : Results.Problem("Database connection failed");
});

// API 1: Lấy danh sách sự kiện
app.MapGet("/events", async (AppDbContext db) =>
{
    var events = await db.Events
        .AsNoTracking()
        .OrderBy(e => e.Id)
        .ToListAsync();

    return Results.Ok(events);
});

// API 2: Lấy chi tiết sự kiện theo ID
app.MapGet("/events/{id:int}", async (int id, AppDbContext db) =>
{
    var eventItem = await db.Events
        .AsNoTracking()
        .FirstOrDefaultAsync(e => e.Id == id);

    if (eventItem == null)
    {
        return Results.NotFound(new
        {
            message = "Event not found"
        });
    }

    return Results.Ok(eventItem);
});

// Luôn đặt app.Run() ở cuối cùng

app.MapPost("/bookings", async (
    CreateBookingRequest request,
    AppDbContext db) =>
{
    // 1. Kiểm tra số lượng vé
    if (request.Quantity <= 0)
    {
        return Results.BadRequest(new
        {
            message = "Quantity must be greater than 0"
        });
    }

    // 2. Kiểm tra sự kiện tồn tại
    var eventItem = await db.Events
        .FirstOrDefaultAsync(e => e.Id == request.EventId);

    if (eventItem == null)
    {
        return Results.NotFound(new
        {
            message = "Event not found"
        });
    }

    // 3. Kiểm tra người dùng tồn tại
    var userExists = await db.Users
        .AnyAsync(u => u.Id == request.UserId);

    if (!userExists)
    {
        return Results.NotFound(new
        {
            message = "User not found"
        });
    }

    // 4. Tạo yêu cầu đặt vé
    var booking = new Booking
    {
        EventId = request.EventId,
        UserId = request.UserId,
        Quantity = request.Quantity,
        Status = "Pending",
        CreatedAt = DateTime.UtcNow
    };

    // 5. Lưu vào SQL Server
    db.Bookings.Add(booking);
    await db.SaveChangesAsync();

    // 6. Trả về kết quả
    return Results.Created(
        $"/bookings/{booking.Id}",
        new
        {
            booking.Id,
            booking.EventId,
            booking.UserId,
            booking.Quantity,
            booking.Status,
            booking.CreatedAt
        });
});

app.MapGet("/bookings/{id:int}", async (
    int id,
    AppDbContext db) =>
{
    var booking = await db.Bookings
        .AsNoTracking()
        .FirstOrDefaultAsync(b => b.Id == id);

    if (booking == null)
    {
        return Results.NotFound(new
        {
            message = "Booking not found"
        });
    }

    return Results.Ok(booking);
});


app.MapGet("/users/{userId:int}/bookings", async (
    int userId,
    AppDbContext db) =>
{
    var userExists = await db.Users
        .AnyAsync(u => u.Id == userId);

    if (!userExists)
    {
        return Results.NotFound(new
        {
            message = "User not found"
        });
    }

    var bookings = await db.Bookings
        .AsNoTracking()
        .Where(b => b.UserId == userId)
        .OrderByDescending(b => b.CreatedAt)
        .ToListAsync();

    return Results.Ok(bookings);
});

app.Run();
