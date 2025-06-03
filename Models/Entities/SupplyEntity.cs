namespace SUPIR_RAZOR.Models.Entities
{
    public class SupplyEntity
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; } = 0;

        public DateTime Date { get; set; }

        public Guid MasterId { get; set; }
        public MasterEntity? Master { get; set; }

        public Guid ProductId { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
