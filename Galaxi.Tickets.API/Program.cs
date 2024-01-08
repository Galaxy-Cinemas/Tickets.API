using System.Reflection;
using Galaxi.Tickets.Persistence;
using Galaxi.Tickets.Persistence.Persistence;

//using Galaxi.Tickets.Domain.Profiles;
using Galaxi.Tickets.Persistence.Repositorys;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var service = builder.Services.BuildServiceProvider();
var configuration = service.GetService<IConfiguration>();


builder.Services.AddInfrastructure(configuration);
//builder.Services.AddAutoMapper(typeof(MovieProfile).Assembly);
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
//builder.Services.AddMediatR(Assembly.Load("Galaxi.Movie.Domain"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

ApplyMigration();

app.MapControllers();

app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<TicketContextDb>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
