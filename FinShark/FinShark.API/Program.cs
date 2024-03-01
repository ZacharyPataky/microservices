using FinShark.DAL.Models;
using FinShark.Domain.Stock;
using FinShark.Domain.Comment;
using Microsoft.EntityFrameworkCore;
using FinShark.Domain.Interfaces;
using FinShark.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfiles_Stock));
builder.Services.AddAutoMapper(typeof(MappingProfiles_Comment));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Hook up to our database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
