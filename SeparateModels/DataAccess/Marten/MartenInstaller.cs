using Marten;
using Marten.Services;
using Microsoft.Extensions.DependencyInjection;
using SeparateModels.Domain;

namespace SeparateModels.DataAccess.Marten
{
    public static class MartenInstaller
    {
        public static void AddMarten(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton<IDocumentStore>(CreateDocumentStore(cnnString));

            services.AddScoped<IDataStore, MartenDataStore>();
        }

        private static IDocumentStore CreateDocumentStore(string cn)
        {
            return DocumentStore.For(_ =>
            {
                _.Connection(cn);
                _.Serializer(CustomizeJsonSerializer());

                _.Schema.For<Policy>().Duplicate(t => t.Number,pgType: "varchar(50)", configure: idx => idx.IsUnique = true);
                _.Schema.For<Offer>().Duplicate(t => t.Number,pgType: "varchar(50)", configure: idx => idx.IsUnique = true);
                _.Schema.For<Product>().Duplicate(t => t.Code,pgType: "varchar(50)", configure: idx => idx.IsUnique = true);
            });
        }

        private static JsonNetSerializer CustomizeJsonSerializer()
        {
            var serializer = new JsonNetSerializer();

            serializer.Customize(_ =>
            {
                _.ContractResolver = new ProtectedSettersContractResolver();
            });

            return serializer;
        }
    }
}