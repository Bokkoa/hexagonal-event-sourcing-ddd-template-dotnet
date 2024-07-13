using Application.Abstractions.Ports.Contracts;
using Application.Abstractions.Ports.Handlers;
using Application.Abstractions.Ports.Repositories;
using Application.Commands;
using Application.Dispatcher;
using Application.Dispatchers;
using Application.Handlers;
using Application.Queries;
using Domain.Models;
using Domain.Modules.Todos.Aggregates;
using Infrastructure.Common.Repositories;
using Infrastructure.Kafka;
using Infrastructure.MongoDb;
using Infrastructure.Postgresql;


var builder = WebApplication.CreateBuilder(args);

// +==============+
// ||  ADAPTERS  ||
// +==============+

builder.Services.InjectMongoDbDependency(builder.Configuration);
builder.Services.InjectPostgresqlDependency(builder.Configuration);


// KAFKA
builder.Services.InjectKafkaDependency(builder.Configuration);

// DDD
builder.Services.AddScoped<IEventHandler, Application.Handlers.EventHandler>();
builder.Services.AddScoped<IEventSourcingHandler<TodoAggregate>, EventSourcingHandler<TodoAggregate>>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();
builder.Services.AddScoped<IQueryHandler, QueryHandler>();
// COMMAND HANDLERS
var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var commandDispatcher = new CommandDispatcher();

commandDispatcher.RegisterHandler<NewTodoCommand>(commandHandler.HandleAsync);


// QUERY HANDLERS
var queryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
var querydispatcher = new QueryDispatcher<TodoModel>();
querydispatcher.RegisterHandler<FindAllTodosQuery>(queryHandler.HandleAsync);

// REPOSITORIES
builder.Services.AddScoped<IFooRepository, FooRepository>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

// DISPATCHERS
builder.Services.AddSingleton<ICommandDispatcher>(_ => commandDispatcher);
builder.Services.AddSingleton<IQueryDispatcher<TodoModel>>(_ => querydispatcher);


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
