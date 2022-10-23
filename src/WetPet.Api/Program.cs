using WetPet.Api.DependencyInjection;
using WetPet.AppCore.DependencyInjection;
using WetPet.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddApi(builder.Configuration, builder.Environment)
    .AddAppCore()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}
app.UseHttpsRedirection();
app.UseCustomCors();
app.UseAuth();
app.MapControllers();
app.Run();
