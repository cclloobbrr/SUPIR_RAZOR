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

        //проверка
        public async Task<CustomerEntity?> CheckCustomer(string name, string phoneNumber)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == name && c.PhoneNumber == phoneNumber);
        }

        //проверка
        public async Task<CustomerEntity> GetCustomer(string name, string phoneNumber)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == name && c.PhoneNumber == phoneNumber);
        }

        //
        public async Task AddCustomer(Guid id, string name, string phoneNumber)
        {
            var customerEntity = new CustomerEntity
            {
                Id = id,
                Name = name,
                PhoneNumber = phoneNumber
            };

            await _dbContext.AddAsync(customerEntity);
            await _dbContext.SaveChangesAsync();
        }

        //Добавление
        public async Task Add(Guid id, DateTime date, bool status, Guid customerId, Dictionary<Guid, int> orderQuantities)
        {
            if (orderQuantities == null || !orderQuantities.Any())
                throw new ArgumentException("Список товаров не может быть пустым");

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // 1. Создаём заказ
                var orderEntity = new OrderEntity
                {
                    Id = id,
                    Date = date,
                    Status = status,
                    CustomerId = customerId
                };
                await _dbContext.AddAsync(orderEntity);
                await _dbContext.SaveChangesAsync(); // Сохраняем заказ сразу

                // 2. Обрабатываем товары
                foreach (var (productId, quantity) in orderQuantities)
                {
                    if (quantity <= 0)
                        continue;

                    // Проверяем существование товара
                    var product = await _dbContext.Products.FindAsync(productId);
                    if (product == null)
                        throw new Exception($"Товар с ID {productId} не найден");

                    // Проверяем достаточность количества
                    if (product.Quantity < quantity)
                        throw new Exception($"Недостаточно товара {product.Name} (доступно: {product.Quantity}, требуется: {quantity})");

                    // Обновляем количество товара
                    product.Quantity -= quantity;

                    // Создаём связь многие-ко-многим
                    var productOrder = new ProductOrderEntity
                    {
                        OrderId = id,
                        ProductId = productId,
                        Quantity = quantity
                    };
                    await _dbContext.AddAsync(productOrder);
                }

                // 3. Сохраняем все изменения
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Ошибка при создании заказа", ex);
            }
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
