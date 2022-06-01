namespace API.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }
        public string PictureUrl { get; set; }
        public DateTime RegDT { get; set; }
        public DateTime UpDT { get; set; }
        public bool IsActive { get; set; }
    }
}