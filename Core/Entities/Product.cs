namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set; }
        public string Desc { get; set; }
        public DateTime RegDT { get; set; }
        public DateTime UpDT { get; set; }
        public bool IsActive { get; set; }
    }
}