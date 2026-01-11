using Microsoft.EntityFrameworkCore;
using ClinicManagement.API.Models; 

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ClinicManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClinicDB")));

// 2. Cấu hình CORS (Cho phép Web React chạy ở localhost:5173 gọi vào)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            // Cho phép Frontend ở cổng 5173 gọi API
            policy.WithOrigins("http://localhost:5173") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();
app.Run();


