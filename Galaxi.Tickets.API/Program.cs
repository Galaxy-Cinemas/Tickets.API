using System.Reflection;
using System.Text;
using FluentValidation;
using Galaxi.Bus.Message;
using Galaxi.Tickets.Domain.DTOs;
using Galaxi.Tickets.Domain.IntegrationEvents.Validators;
using Galaxi.Tickets.Domain.Profiles;
using Galaxi.Tickets.Domain.Services;
using Galaxi.Tickets.Persistence;
using Galaxi.Tickets.Persistence.Persistence;
using Galaxi.Tickets.Persistence.Repositorys;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var service = builder.Services.BuildServiceProvider();
var configuration = service.GetService<IConfiguration>();

builder.Services.AddMassTransit(x =>
{
    x.AddRequestClient<CheckFunctionSeats>();
    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(configuration.GetConnectionString("AzureServiceBus"));

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddInfrastructure(configuration);
builder.Services.AddAutoMapper(typeof(TicketProfile).Assembly);
builder.Services.AddMediatR(Assembly.Load("Galaxi.Tickets.Domain"));

builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketServices, TicketServices>();
builder.Services.AddScoped<IValidator<TicketDto>, ValidatorAvailableSeats>();


// Add Authentication
var secretKey = Encoding.ASCII.GetBytes(
    builder.Configuration.GetValue<string>("SecretKey")
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.WithOrigins(new[]
        {
                        "http://localhost:4200",
                        "http://localhost:50928"
        })
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseAuthentication();

//ApplyMigration();

app.MapControllers();

app.Run();

//void ApplyMigration()
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var _db = scope.ServiceProvider.GetRequiredService<TicketContextDb>();

//        if (_db.Database.GetPendingMigrations().Count() > 0)
//        {
//            _db.Database.Migrate();
//        }
//    }
//}
