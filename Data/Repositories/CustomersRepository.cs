using Microsoft.EntityFrameworkCore;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Data.Repositories
{
    public class CustomersRepository
    {
        private readonly SUPIR_RAZORDbContext _dbContext;

        public CustomersRepository(SUPIR_RAZORDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Получение
        public async Task<List<CustomerEntity>> GetAll()
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<CustomerEntity>> GetAllPhoneNumberSortASC()
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .OrderBy(c => c.PhoneNumber)
                .ToListAsync();
        }

        public async Task<List<CustomerEntity>> GetAllPhoneNumberSortDESC()
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .OrderByDescending(c => c.PhoneNumber)
                .ToListAsync();
        }

        public async Task<List<CustomerEntity>> GetAllNameSortASC()
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<List<CustomerEntity>> GetAllNameSortDESC()
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .OrderByDescending(c => c.Name)
                .ToListAsync();
        }

        public async Task<CustomerEntity?> GetById(Guid id)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<CustomerEntity>> GetByName(String name)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .Where(c => c.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<List<CustomerEntity>> GetByPhoneNumber(string phoneNumber)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .Where(c => c.PhoneNumber.Contains(phoneNumber))
                .ToListAsync();
        }

        //Добавление
        public async Task Add(Guid id, string name, string phoneNumber)
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

        //Обновление
        public async Task Update(CustomerEntity customer)
        {
            _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync();
        }

        //Удаление
        public async Task<DeleteResult> Delete(Guid id)
        {
            var customer = await _dbContext.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(customer == null)
            {
                return DeleteResult.NotFound;
            }
            if (customer.Orders?.Any() == true)
            {
                return DeleteResult.HasDependencies;
            }

            _dbContext.Customers.Remove(customer);
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