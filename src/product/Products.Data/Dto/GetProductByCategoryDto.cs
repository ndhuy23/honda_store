namespace ProductService.Data.Dto
{
    public class GetProductByCategoryDto
    {
        public string CategoryName { get; set; }

        public int Page {  get; set; }

        public int PageSize { get; set; }

    }
}
