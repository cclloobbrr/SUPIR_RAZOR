namespace SUPIR_RAZOR.Models.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public bool Status { get; set; }

        public List<ProductOrderEntity> ProductsOrders { get; set; } = [];

        public Guid CustomerId { get; set; }

        public CustomerEntity? Customer { get; set; }
    }
}
