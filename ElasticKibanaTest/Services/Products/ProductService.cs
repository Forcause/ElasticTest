using ElasticTest.Models;
using ElasticTest.Services.Products.Models;
using Nest;

namespace ElasticTest.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IElasticClient _elasticClient;

        public ProductService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<bool> IndexProduct(Product product)
        {
            var response = await _elasticClient.IndexDocumentAsync(product);

            return response.IsValid;
        }

        public async Task<List<Product>> Search(SearchProductRequest request)
        {
            var searchDescriptor = new SearchRequest<Product>()
            {
                Query = GetQueryFromRequest(request)
            };

            if (request != null)
            {
                searchDescriptor.From = request.Skip;
                searchDescriptor.Size = request.Take;
            }

            if (request.TermFilter != null)
            {
                searchDescriptor.PostFilter = GetTermFiltersFromRequest(request.TermFilter);
            }

            var results = await _elasticClient.SearchAsync<Product>(searchDescriptor);

            return results.Documents.ToList();
        }

        private QueryContainer GetTermFiltersFromRequest(TermFilter termFilter)
        {
            var termValues = termFilter.Values;

            return new TermsQuery
            {
                Field = termFilter.FieldName.ToLowerInvariant(),
                Terms = termValues
            };
        }

        private QueryContainer GetQueryFromRequest(SearchProductRequest request)
        {
            QueryContainer result = null;

            if (!string.IsNullOrEmpty(request?.SearchKeywords))
            {
                var keywords = request.SearchKeywords;
                var fields = request.SearchFields?.Select(x => x.ToLowerInvariant()).ToArray() ?? new[] { "_all" };

                var multiMatch = new QueryStringQuery
                {
                    Fields = fields,
                    Query = $"*{ keywords }*",
                    Type = TextQueryType.Phrase,
                    Analyzer = "standard",
                };

                result = multiMatch;
            }

            return result;
        }
    }
}
