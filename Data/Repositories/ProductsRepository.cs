using Microsoft.EntityFrameworkCore;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Data.Repositories
{
    public class ProductsRepository
    {
        private readonly SUPIR_RAZORDbContext _dbContext;

        public ProductsRepository(SUPIR_RAZORDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Получение
        public async Task<List<ProductEntity>> GetAll()
        {
            return await _dbContext.Products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductEntity?> GetById(Guid id)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProductEntity?> GetByName(string name)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == name);
        }
        public async Task<ProductEntity?> GetByMaterial(string material)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Material == material);
        }

        public async Task<ProductEntity?> GetByPrice(decimal price)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Price == price);
        }

        //Добавление
        public async Task Add(Guid id, string name, string material, decimal price, int quantity)
        {
            var productEntity = new ProductEntity
            {
                Id = id,
                Name = name,
                Material = material,
                Price = price,
                Quantity = quantity
            };

            await _dbContext.AddAsync(productEntity);
            await _dbContext.SaveChangesAsync();
        }

        //Обновление
        public async Task Update(Guid id, string name, string material, decimal price, int quantity)
        {
            await _dbContext.Products
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Name, name)
                .SetProperty(p => p.Material, material)
                .SetProperty(p => p.Price, price)
                .SetProperty(p => p.Quantity, quantity));
        }

        //Удаление
        public async Task Delete(Guid id)
        {
            if (!await _dbContext.Products.AnyAsync(p => p.Id == id))
                throw new ArgumentException("A non-existing ID", nameof(id));

            await _dbContext.Products
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
