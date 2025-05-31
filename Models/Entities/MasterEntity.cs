namespace SUPIR_RAZOR.Models.Entities
{
    public class MasterEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public List<SupplyEntity> Supplies { get; set; } = [];
    }
}
