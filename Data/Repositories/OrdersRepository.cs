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
        public async Task Add(Guid id, DateTime date, bool status, Guid customerId)
        {
            var orderEntity = new OrderEntity
            {
                Id = id,
                Date = date,
                Status = status,
                CustomerId = customerId
            };

            await _dbContext.AddAsync(orderEntity);
            await _dbContext.SaveChangesAsync();
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