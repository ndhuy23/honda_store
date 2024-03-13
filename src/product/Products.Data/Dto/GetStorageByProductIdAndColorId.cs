namespace ProductService.Data.Dto
{
    public class GetStorageByProductIdAndColorId
    {
        public Guid ProductId { get; set; }

        public Guid ColorId { get; set; }
    }
}
