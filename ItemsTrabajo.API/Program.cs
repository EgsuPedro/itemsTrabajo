using ItemsTrabajo.BLL.Services;
using ItemsTrabajo.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositorios y Servicios
builder.Services.AddSingleton<IWorkItemRepository, WorkItemRepository>();
builder.Services.AddScoped<IWorkItemService, WorkItemService>();

// Configuración del Cliente HTTP para comunicarse con GestionUsuarios.API
builder.Services.AddHttpClient<IGestionUsuariosClient, GestionUsuariosClient>(client =>
{
    // Cambiar el puerto según el puerto donde corra GestionUsuarios.API en tu máquina (ej. 5001 o 7123)
    //  client.BaseAddress = new Uri("https://localhost:7123/");
    client.BaseAddress = new Uri("https://localhost:7147/");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();