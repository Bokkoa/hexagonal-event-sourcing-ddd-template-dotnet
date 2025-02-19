﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.MongoDb.Config;
using MongoDB.Driver;
using Application.Abstractions.Ports.Repositories;
using Infrastructure.MongoDb.Events;
using Infrastructure.MongoDb.Repositories;
using Application.Abstractions.Ports.Contracts;
using Infrastructure.MongoDb.Stores;

namespace Infrastructure.MongoDb;
public static class InfrastructureMongoDbAdapterDependency
{

    public static IServiceCollection InjectMongoDbDependency(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbConfig = new MongoDbConfig();
        var configSection = configuration.GetSection("MongoDbConfig");
        configSection.Bind(mongoDbConfig);

        // Register IMongoDB Instance
        services.AddSingleton<IMongoClient, MongoClient>(sp =>
        {
            return new MongoClient(mongoDbConfig.ConnectionString);
        });

        // Register the IMongoDatabase instance
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoDbConfig.Database);
        });

        // Register IMongoClientCollection instance
        services.AddSingleton(sp =>
        {
            var database = sp.GetRequiredService<IMongoDatabase>();

            return database.GetCollection<MongoDbEventModel>(mongoDbConfig.Collection);
        });

        // STORE
        services.AddScoped<IEventStore, EventStore>();

        // Repository implementation
        services.AddScoped<IEventStoreRepository, EventStoreRepository>();

        return services;

    }

}
