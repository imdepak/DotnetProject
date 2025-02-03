
using MyDotnetProject.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<DemoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular app's origin
              .AllowAnyHeader()                   // Allow all headers
              .AllowAnyMethod();                  // Allow all HTTP methods (GET, POST, etc.)
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }
// Enable CORS for the app
app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication(); // Validates credentials
app.UseAuthorization(); 
app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

app.Run();


