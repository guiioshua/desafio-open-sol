using Microsoft.EntityFrameworkCore;
using DesafioGerenciadorTarefas.Infrastructure.Data;
using DesafioGerenciadorTarefas.Core.Interfaces;
using DesafioGerenciadorTarefas.Infrastructure.Repositories;
using DesafioGerenciadorTarefas.Core.Services;
using DesafioGerenciadorTarefas.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// TODO: Auto-migration apenas para desenvolvimento. Em produção, usaria pipeline CI/CD
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    db.Database.Migrate();
}

// TODO: Swagger não é exposto em prod, só para fins do desafio
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionMiddleware>();

// app.UseAuthorization();

app.MapControllers();

app.Run();