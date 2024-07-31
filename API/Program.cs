using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(opt => 
{
   opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultString"));
});
var app = builder.Build();
app.MapControllers();
app.Run();
