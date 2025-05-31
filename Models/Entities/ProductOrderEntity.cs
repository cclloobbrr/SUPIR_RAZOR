namespace SUPIR_RAZOR.Models.Entities
{
    public class ProductOrderEntity
    {
        public int Quantity { get; set; }

        public Guid ProductId { get; set; }

        public ProductEntity? Product { get; set; }

        public Guid OrderId { get; set; }

        public OrderEntity? Order { get; set; }
    }
}
