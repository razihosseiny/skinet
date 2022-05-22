using System;

namespace Core.Entities
{
    public class ProductPrice : BaseEntity
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public DateTime RegDT { get; set; }
        public bool IsActive { get; set; }

    }
}