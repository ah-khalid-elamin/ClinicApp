using ClinicApp.DbContexts;
using ClinicApp.Services;
using ClinicApp.Services.Impl;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//appointments
builder.Services.AddDbContext<ClinicAppDbContext>(opt =>
    opt.UseSqlServer("Data Source=localhost;Initial Catalog=ClinicApp;Trusted_Connection=true"));

builder.Services.AddScoped<PatientService, PatientServiceImpl>();
builder.Services.AddScoped<DoctorService, DoctorServiceImpl>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
