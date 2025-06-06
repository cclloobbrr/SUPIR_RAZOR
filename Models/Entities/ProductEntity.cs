﻿namespace SUPIR_RAZOR.Models.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty; 

        public string Material { get; set; } = string.Empty;

        public decimal Price { get; set; } = 0;

        public int Quantity { get; set; } = 0;

        public List<SupplyEntity> Supplies { get; set; } = [];

        public List<ProductOrderEntity> ProductsOrders { get; set; } = [];
    }
}
