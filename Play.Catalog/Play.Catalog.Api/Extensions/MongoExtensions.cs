﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Play.Catalog.Api.Helpers;
using Play.Catalog.Application.Interfaces;
using Play.Catalog.Infrastructure.Repositories;

namespace Play.Catalog.Api.Extensions
{
    public static class MongoExtensions
    {
        public static IServiceCollection AddMongoOptions(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            return services;
        }

        public static IServiceCollection AddMongoRepository(this IServiceCollection services, string collectionName)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.AddSingleton<IMongoDbConfig>(_ => new MongoDbConfig(configuration, collectionName));
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

            return services;
        }
    }
}
