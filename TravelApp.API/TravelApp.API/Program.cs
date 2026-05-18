using TravelApp.API.Repositories;
using TravelApp.API.Services;

var builder = WebApplication.CreateBuilder(args);

// DB 연결 문자열
string connStr = "Server=localhost;Database=TravelKoreaDB;Trusted_Connection=True;TrustServerCertificate=True;";

// 의존성 주입 등록
builder.Services.AddSingleton(new AccommodationRepository(connStr));
builder.Services.AddSingleton(new AccommodationService(
    new AccommodationRepository(connStr)
));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();