using Microsoft.EntityFrameworkCore;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Data.Repositories
{
    public class MastersRepository
    {
        private readonly SUPIR_RAZORDbContext _dbContext;

        public MastersRepository(SUPIR_RAZORDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Получение
        public async Task<List<MasterEntity>> GetAll()
        {
            return await _dbContext.Masters
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<MasterEntity>> GetAllNameSortASC()
        {
            return await _dbContext.Masters
                .AsNoTracking()
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<List<MasterEntity>> GetAllNameSortDESC()
        {
            return await _dbContext.Masters
                .AsNoTracking()
                .OrderByDescending(m => m.Name)
                .ToListAsync();
        }

        public async Task<List<MasterEntity>> GetAllPhoneNumberSortASC()
        {
            return await _dbContext.Masters
                .AsNoTracking()
                .OrderBy(m => m.PhoneNumber)
                .ToListAsync();
        }

        public async Task<List<MasterEntity>> GetAllPhoneNumberSortDESC()
        {
            return await _dbContext.Masters
                .AsNoTracking()
                .OrderByDescending(m => m.PhoneNumber)
                .ToListAsync();
        }

        public async Task<List<MasterEntity>> GetWithSupplies()
        {
            return await _dbContext.Masters
                .AsNoTracking()
                .Include(m => m.Supplies)
                .ToListAsync();
        }

        public async Task<MasterEntity?> GetById(Guid id)
        {
            return await _dbContext.Masters
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<MasterEntity>> GetByName(String name)
        {
            return await _dbContext.Masters
                .AsNoTracking()
                .Where(m => m.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<List<MasterEntity>> GetByPhoneNumber(string phoneNumber)
        {
            return await _dbContext.Masters
                .AsNoTracking()
                .Where(m => m.PhoneNumber.Contains(phoneNumber))
                .ToListAsync();
        }

        public async Task<List<MasterEntity>> GetByFilter(string name, string phoneNumber)
        {
            var query = _dbContext.Masters.AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                query = query.Where(m => m.PhoneNumber.Contains(phoneNumber));
            }

            return await query.ToListAsync();
        }

        //Добавление
        public async Task Add(Guid id, string name, string phoneNumber)
        {
            var masterEntity = new MasterEntity
            {
                Id = id,
                Name = name,
                PhoneNumber = phoneNumber
            };

            await _dbContext.AddAsync(masterEntity);
            await _dbContext.SaveChangesAsync();
        }

        //Обновление
        public async Task Update(MasterEntity master)
        {
            _dbContext.Masters.Update(master);
            await _dbContext.SaveChangesAsync();
        }

        //Удаление
        public async Task<DeleteResult> Delete(Guid id)
        {
            var master = await _dbContext.Masters
                .Include(m => m.Supplies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if(master == null)
            {
                return DeleteResult.NotFound;
            }
            if(master.Supplies?.Any() == true)
            {
                return DeleteResult.HasDependencies;
            }

            _dbContext.Masters.Remove(master);
            await _dbContext.SaveChangesAsync();
            return DeleteResult.Success;
        }

        public enum DeleteResult
        {
            Success,
            NotFound,
            HasDependencies
        }
    }
}
