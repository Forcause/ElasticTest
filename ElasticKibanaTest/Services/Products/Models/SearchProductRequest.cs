namespace ElasticTest.Services.Products.Models
{
    public class SearchProductRequest
    {
        public int Skip { get; set; }

        public int Take { get; set; } = 20;

        public string SearchKeywords { get; set; }

        public List<string>? SearchFields { get; set; }

        public TermFilter? TermFilter { get; set; }
    }

    public class TermFilter
    {
        public string FieldName { get; set; }
        public IList<string> Values { get; set; }

        public override string ToString()
        {
            return $"{FieldName}:{string.Join(",", Values)}";
        }
    }
}
