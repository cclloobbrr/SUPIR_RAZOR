namespace SUPIR_RAZOR.Models.Entities
{
    public class CustomerEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public List<OrderEntity> Orders { get; set; } = [];
    }
}
