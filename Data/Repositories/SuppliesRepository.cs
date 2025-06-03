using Microsoft.EntityFrameworkCore;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Data.Repositories
{
    public class SuppliesRepository
    {
        private readonly SUPIR_RAZORDbContext _dbContext;

        public SuppliesRepository(SUPIR_RAZORDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Получение
        public async Task<List<SupplyEntity>> GetAll()
        {
            return await _dbContext.Supplies
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SupplyEntity?> GetById(Guid id)
        {
            return await _dbContext.Supplies
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<SupplyEntity?> GetByDate(DateTime date)
        {
            return await _dbContext.Supplies
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Date.Date == date.Date);
        }
        public async Task<SupplyEntity?> GetByMasterId(Guid masterId)
        {
            return await _dbContext.Supplies
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.MasterId == masterId);
        }

        public async Task<SupplyEntity?> GetByProductId(Guid productId)
        {
            return await _dbContext.Supplies
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ProductId == productId);
        }

        //Добавление
        public async Task Add(Guid id, int quantity, DateTime date, Guid masterId, Guid productId)
        {
            var supplyEntity = new SupplyEntity
            {
                Id = id,
                Quantity = quantity,
                Date = date,
                MasterId = masterId,
                ProductId = productId
            };

            await _dbContext.AddAsync(supplyEntity);
            await _dbContext.SaveChangesAsync();
        }

        //Обновление
        public async Task Update(Guid id, int quantity, DateTime date, Guid masterId, Guid productId)
        {
            await _dbContext.Supplies
                .Where(s => s.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(s => s.Quantity, quantity)
                .SetProperty(s => s.Date, date)
                .SetProperty(s => s.MasterId, masterId)
                .SetProperty(s => s.ProductId, productId));
        }

        //Удаление
        public async Task Delete(Guid id)
        {
            if (!await _dbContext.Supplies.AnyAsync(s => s.Id == id))
                throw new ArgumentException("A non-existing ID", nameof(id));

            await _dbContext.Supplies
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
