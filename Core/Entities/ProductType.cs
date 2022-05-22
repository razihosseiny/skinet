namespace Core.Entities
{
    public class ProductType : BaseEntity
    {
        public int? ParentID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}