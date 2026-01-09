using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Infrastructure.DbContexts;

public class AppDbContext  
{

    private readonly IMongoDatabase _database;
    public AppDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb")
            ?? "mongodb://localhost:27017";
        var databaseName = configuration["MongoDb:DatabaseName"] ?? "CategoriesDB";

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<CategorieDbModel> Categories =>
        _database.GetCollection<CategorieDbModel>("categories");
}