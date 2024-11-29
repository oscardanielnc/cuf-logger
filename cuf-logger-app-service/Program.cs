using cuf_admision_domain.Services;
using cuf_admision_data.Configuration;
using Microsoft.EntityFrameworkCore;
using cuf_admision_data.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// --- Dependecy Injections ---
// services
builder.Services.AddScoped<IUtilsService, LogTools>();

// repositories
/*builder.Services.AddScoped<IAdmisionFlowRepository, AdmisionFlowRepositoryImpl>();*/

// BD

// CORS
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(
        policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.UseCors();
app.Run();

