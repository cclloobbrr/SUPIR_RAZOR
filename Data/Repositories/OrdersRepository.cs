using Microsoft.EntityFrameworkCore;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Data.Repositories
{
    public class OrdersRepository
    {
        private readonly SUPIR_RAZORDbContext _dbContext;

        public OrdersRepository(SUPIR_RAZORDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Получение
        public async Task<List<OrderEntity>> GetAll()
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .ToListAsync();
        }

        public async Task<OrderEntity?> GetById(Guid id)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        //Добавление
        public async Task Add(Guid id, DateTime date, bool status, Guid customerId, Dictionary<Guid, int> orderQuantities)
        {
            // Начало транзакции
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // добавление в Orders
                var orderEntity = new OrderEntity
                {
                    Id = id,
                    Date = date,
                    Status = status,
                    CustomerId = customerId
                };
                await _dbContext.AddAsync(orderEntity);
                await _dbContext.SaveChangesAsync();

                // Обновление Products
                foreach(var orderProduct in orderQuantities)
                {
                    Guid productId = orderProduct.Key;
                    int productQuantity = orderProduct.Value;

                    await _dbContext.Products
                        .Where(p => p.Id == productId)
                        .ExecuteUpdateAsync(p => p.SetProperty(x => x.Quantity, x => x.Quantity - productQuantity));
                }
                await _dbContext.SaveChangesAsync();

                foreach (var orderProduct in orderQuantities)
                {
                    Guid productId = orderProduct.Key;
                    int productQuantity = orderProduct.Value;
                    if(productQuantity > 0)
                    {
                        var m2m = new ProductOrderEntity
                        {
                            OrderId = id,
                            ProductId = productId,
                            Quantity = productQuantity
                        };
                        await _dbContext.AddAsync(m2m);
                    }
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        //Обновление
        public async Task Update(Guid id, DateTime date, bool status, Guid customerId)
        {
            await _dbContext.Orders
                .Where(o => o.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(o => o.Date, date)
                .SetProperty(o => o.Status, status)
                .SetProperty(o => o.CustomerId, customerId));
        }

        //Удаление
        public async Task Delete(Guid id)
        {
            if (!await _dbContext.Orders.AnyAsync(c => c.Id == id))
                throw new ArgumentException("A non-existing ID", nameof(id));

            await _dbContext.Orders
                .Where(o => o.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}