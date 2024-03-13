using ProductService.Data.Entities;
using ProductService.Data.Template;

namespace ProductService.Data.Response
{
    public class ProductCategoryResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }

        public Color Colors { get; set; }

        public List<Detail> Details { get; set; }
    }
}
