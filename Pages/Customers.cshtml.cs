using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SUPIR_RAZOR.Data.Repositories;
using SUPIR_RAZOR.Models.Entities;
using static SUPIR_RAZOR.Data.Repositories.CustomersRepository;

namespace SUPIR_RAZOR.Pages
{
    public class CustomersModel : PageModel
    {
        private readonly CustomersRepository _repository;

        public CustomersModel(CustomersRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public List<CustomerEntity> Customers { get; set; } = new();

        [BindProperty]
        public CustomerEntity NewCustomer { get; set; } = new();

        [BindProperty]
        public CustomerEntity UpdateCustomer { get; set; }

        public async Task OnGetAsync()
        {
            Customers = await _repository.GetAll();
        }
        public async Task OnGetSortByNameASCAsync()
        {
            Customers = await _repository.GetAllNameSortASC();
        }

        public async Task OnGetSortByNameDESCAsync()
        {
            Customers = await _repository.GetAllNameSortDESC();
        }

        public async Task OnGetSortByPhoneASCAsync()
        {
            Customers = await _repository.GetAllPhoneNumberSortASC();
        }

        public async Task OnGetSortByPhoneDESCAsync()
        {
            Customers = await _repository.GetAllPhoneNumberSortDESC();
        }

        public async Task OnGetSearchByIdAsync(Guid id)
        {
            var customer = await _repository.GetById(id);
            Customers = customer != null ? new List<CustomerEntity> { customer } : new List<CustomerEntity>();
        }

        public async Task OnGetSearchByNameAsync(string name)
        {
            Customers = await _repository.GetByName(name);
        }

        public async Task OnGetSearchByPhoneAsync(string phone)
        {
            Customers = await _repository.GetByPhoneNumber(phone);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var result = await _repository.Delete(id);

            switch (result)
            {
                case DeleteResult.HasDependencies:
                    TempData["ErrorMessage"] = "Cannot delete customer with existing orders";
                    break;
                case DeleteResult.NotFound:
                    TempData["ErrorMessage"] = "Customer not found";
                    break;
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Customers = await _repository.GetAll();
                return RedirectToPage(); ;
            }

            await _repository.Add(Guid.NewGuid(), NewCustomer.Name, NewCustomer.PhoneNumber);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync(Guid id)
        {
            var existingCustomer = await _repository.GetById(id);
            if (existingCustomer == null)
            {
                ModelState.AddModelError("", "Customer not found");
                return RedirectToPage();
            }

            if (!string.IsNullOrEmpty(UpdateCustomer.Name))
            {
                existingCustomer.Name = UpdateCustomer.Name;
            }

            if (!string.IsNullOrEmpty(UpdateCustomer.PhoneNumber))
            {
                existingCustomer.PhoneNumber = UpdateCustomer.PhoneNumber;
            }

            await _repository.Update(existingCustomer);
            return RedirectToPage();
        }
    }
}
