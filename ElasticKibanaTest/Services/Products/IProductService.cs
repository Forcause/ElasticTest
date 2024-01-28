using ElasticTest.Models;
using ElasticTest.Services.Products.Models;

namespace ElasticTest.Services.Products
{
    public interface IProductService
    {
        public Task<List<Product>> Search(SearchProductRequest request);

        public Task<bool> IndexProduct(Product product);
    }
}
