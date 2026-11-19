
using Microsoft.EntityFrameworkCore;
using Ticket.Api.Data;

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
app.Run();
