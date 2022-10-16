using WetPet.Api.DependencyInjection;
using WetPet.AppCore.DependencyInjection;
using WetPet.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddApi(builder.Configuration)
    .AddAppCore()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCustomCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
