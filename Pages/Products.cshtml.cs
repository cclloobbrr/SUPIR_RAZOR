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
        public List<MasterEntity> Products { get; set; } = new();

        [BindProperty]
        public MasterEntity NewProducts { get; set; } = new();

        [BindProperty]
        public MasterEntity UpdateProducts { get; set; } = new();

        public async Task OnGetAsync()
        {
            Products = await _repository.GetAll();
        }
    }
}
