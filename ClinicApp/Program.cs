using ClinicApp.DbContexts;
using ClinicApp.Models;
using ClinicApp.Services;
using ClinicApp.Services.Impl;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())  //location of the exe file
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);


IConfigurationRoot configuration = configurationBuilder.Build();

var secretKey = configuration["Jwt:Key"];

builder.Services.AddOData();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDBContext>(opt =>
    opt.UseSqlServer(configuration["connectionString"]));

builder.Services.AddDbContext<ClinicAppDbContext>(opt =>
    opt.UseSqlServer(configuration["connectionString"]));

builder.Services.AddScoped<PatientService, PatientServiceImpl>();
builder.Services.AddScoped<DoctorService, DoctorServiceImpl>();
builder.Services.AddScoped<AppointmentService, AppointmentServiceImpl>();


// For Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        options.Password.RequireDigit = false
   )
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        //ValidAudience = configuration["Jwt.ValidAudience"],
        //ValidIssuer = configuration["Jwt.Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

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
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.EnableDependencyInjection();
    endpoints.Expand().Select().Count().Filter().MaxTop(100).SkipToken();
});
app.MapControllers();
app.Run();
