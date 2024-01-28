using ElasticTest.Models;
using Nest;

namespace ElasticTest.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var uri = configuration["ELKConfiguration:Uri"];
            var defaultIndex = configuration["ELKConfiguration:index"];

            var settings = new ConnectionSettings(new Uri(uri))
                .PrettyJson()
                .DefaultIndex(defaultIndex);

            AddDefaultMapping(settings);

            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultMapping(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<Product>(x => x.Ignore(p => p.Id));
        }

        private static void CreateIndex(
            IElasticClient elasticClient, 
            string indexName)
        {
            elasticClient.Indices
                .Create(indexName, x => 
                    x.Map<Product>(
                        y => y.AutoMap()));
        }
    }
}
