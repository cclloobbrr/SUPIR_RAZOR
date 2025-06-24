using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SUPIR_RAZOR.Data.Repositories;
using SUPIR_RAZOR.Models.Entities;
using System.Diagnostics.Metrics;

namespace SUPIR_RAZOR.Pages
{
    public class Place_an_orderModel : PageModel
    {
        private readonly ProductsRepository _repository;

        public Place_an_orderModel(ProductsRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public List<ProductEntity> Products { get; set; } = new();

        [BindProperty]
        public Dictionary<Guid, int> OrderQuantities { get; set; } = new();

        [BindProperty]
        public CustomerEntity Customer { get; set; } = new();

        public async Task OnGetAsync()
        {
            Products = await _repository.GetAll();

            foreach(var product in Products)
            {
                OrderQuantities[product.Id] = 0;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Products = await _repository.GetAll();
                return Page();
            }

            // Фильтруем товары с количеством > 0
            var selectedProducts = OrderQuantities
                .Where(x => x.Value > 0)
                .ToDictionary(x => x.Key, x => x.Value);

            if (!selectedProducts.Any())
            {
                ModelState.AddModelError(string.Empty, "Выберите хотя бы один товар");
                Products = await _repository.GetAll();
                return Page();
            }

            try
            {
                var customer = await _repository.CheckCustomer(Customer.Name, Customer.PhoneNumber);

                if (customer == null)
                {
                    var newCustomerId = Guid.NewGuid();
                    await _repository.AddCustomer(newCustomerId, Customer.Name, Customer.PhoneNumber);
                    customer = await _repository.GetCustomer(Customer.Name, Customer.PhoneNumber);
                }

                await _repository.Add(
                    Guid.NewGuid(),
                    DateTime.Now,
                    false,
                    customer.Id,
                    selectedProducts
                );

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ошибка: {ex.Message}");
                Products = await _repository.GetAll();
                return Page();
            }
        }
    }
}
