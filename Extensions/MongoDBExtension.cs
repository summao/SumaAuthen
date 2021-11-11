using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Suma.Authen.Entities;
using Suma.Authen.Helpers;

namespace Suma.Authen.Extensions
{
    public static class MongoDBExtension
    {
        public static void AddMongoDb(this IServiceCollection services, MongoDbSettings settings)
        {
             services.AddSingleton<IMongoDatabase>(_ =>
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.DatabaseName);

                var accountCollection = database.GetCollection<Account>(nameof(Account));
                accountCollection.Indexes.CreateOneAsync(
                    new CreateIndexModel<Account>(
                        new IndexKeysDefinitionBuilder<Account>()
                        .Ascending(new StringFieldDefinition<Account>(nameof(Account.MobileNumber))
                    ),
                    new CreateIndexOptions { Unique = true, })
                ).Wait();
                accountCollection.Indexes.CreateOneAsync(
                    new CreateIndexModel<Account>(
                        new IndexKeysDefinitionBuilder<Account>()
                        .Ascending(new StringFieldDefinition<Account>(nameof(Account.Username))
                    ),
                    new CreateIndexOptions { Unique = true, })
                ).Wait();

                var refreshTokenCollection = database.GetCollection<RefreshToken>(nameof(RefreshToken));
                refreshTokenCollection.Indexes.CreateOneAsync(
                    new CreateIndexModel<RefreshToken>(
                        new IndexKeysDefinitionBuilder<RefreshToken>()
                        .Ascending(new StringFieldDefinition<RefreshToken>(nameof(RefreshToken.Token))
                    ),
                    new CreateIndexOptions { Unique = true, })
                ).Wait();
                refreshTokenCollection.Indexes.CreateOneAsync(
                    new CreateIndexModel<RefreshToken>(
                        new IndexKeysDefinitionBuilder<RefreshToken>()
                        .Ascending(new StringFieldDefinition<RefreshToken>(nameof(RefreshToken.AccountId))
                    ),
                    new CreateIndexOptions { Unique = true, })
                ).Wait();
                return database;
            });
        } 
    }
}