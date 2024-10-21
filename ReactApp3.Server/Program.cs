using MongoDB.Driver;
using Microsoft.Extensions.Options;
using ReactApp3.Server.Services;
using ReactApp3.Server.Models;
using ReactApp3.Server.Helpers;
using ReactApp3.Server.Models.ReactLogin.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("https://localhost:5173")  // Specify your front-end URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Đăng ký UserService với MongoDB
builder.Services.AddSingleton<UserService>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new UserService(database, settings.UsersCollection);
});

// Đăng ký Brevo (SendinBlue) email service
builder.Services.Configure<BrevoSettings>(builder.Configuration.GetSection("BrevoSettings"));
builder.Services.AddTransient<EmailService>();

builder.Services.AddSingleton<TeamService>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return new TeamService(database);
});
builder.Services.AddSingleton<ProjectService>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return new ProjectService(database);
});

// Đăng ký JwtHelper
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<JwtHelper>();

// Thêm Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Cấu hình Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReactApp3 API V1");
});

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
