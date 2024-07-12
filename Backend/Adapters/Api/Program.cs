using Application.Abstractions.Ports.Contracts;
using Application.Abstractions.Ports.Handlers;
using Application.Commands;
using Application.Dispatcher;
using Application.Handlers;
using Confluent.Kafka;
using Domain.Modules.Todos.Aggregates;
using Infrastructure.Kafka;
using Infrastructure.MongoDb;
using Infrastructure.MongoDb.Stores;

var builder = WebApplication.CreateBuilder(args);


// ADAPTERS
builder.Services.InjectMongoDbDependency(builder.Configuration);
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection("KafkaConfig"));

// PRODUCER
builder.Services.AddScoped<IEventProducer, EventProducer>();

// STORE
builder.Services.AddScoped<IEventStore, EventStore>();

// DDD
builder.Services.AddScoped<IEventSourcingHandler<Todo>, EventSourcingHandler<Todo>>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();


// COMMAND HANDLERS
var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();

dispatcher.RegisterHandler<NewTodoCommand>(commandHandler.HandleAsync);

// DISPATCHERS
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);



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
