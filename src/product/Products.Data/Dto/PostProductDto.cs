using ProductService.Data.Entities;
using ProductService.Data.Template;

namespace ProductService.Data.Dto
{
    public class PostProductDto
    {
        public string Name { get; set; }

        public Guid CategoryId { get; set; }

        public Guid CompanyId { get; set; }

        public Dictionary<Guid,int> ColorQuantity { get; set; }

        public Dictionary<Guid, List<string>> ColorImages { get; set; }

        public List<Feature> Features { get; set; }

        public List<Detail> Details { get; set; }

        public List<Preferential> Preferentials { get; set; }

        public List<Preferential> ExtendPreferentials { get; set; }
    }
}
