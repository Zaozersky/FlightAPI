﻿using AirportAPI.Profiles;
using Microsoft.EntityFrameworkCore;
using Airport.DAL.Common;
using AirportAPI.Controllers.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContext, AirportDbContext>
    (o => o.UseNpgsql(builder.Configuration.GetConnectionString("AirportDb")));

builder.Services.AddScoped<IFlightService, FlightService>();

builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(FlightProfile));
builder.Services.AddOptions();

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

