using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SUPIR_RAZOR.Data.Repositories;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly ProductsRepository _repository;

        public ProductsModel(ProductsRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public List<ProductEntity> Products { get; set; } = new();

        [BindProperty]
        public ProductEntity NewProduct { get; set; } = new();

        [BindProperty]
        public ProductEntity UpdateProducts { get; set; } = new();

        public async Task OnGetAsync()
        {
            Products = await _repository.GetAll();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Products = await _repository.GetAll();
                return RedirectToPage(); ;
            }

            await _repository.Add(Guid.NewGuid(), NewProduct.Name, NewProduct.Material, NewProduct.Price, NewProduct.Quantity);
            return RedirectToPage();
        }


    }
}
