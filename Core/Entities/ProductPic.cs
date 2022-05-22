namespace Core.Entities
{
    public class ProductPic : BaseEntity
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }
        public DateTime RegDT { get; set; }
        public bool IsActive { get; set; }
        public string Desc { get; set; }
    }
}