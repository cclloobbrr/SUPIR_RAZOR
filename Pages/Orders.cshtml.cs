using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SUPIR_RAZOR.Data.Repositories;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly OrdersRepository _repository;

        public OrdersModel(OrdersRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public List<OrderEntity> Orders { get; set; } = new();

        [BindProperty]
        public OrderEntity NewOrder { get; set; } = new();

        [BindProperty]
        public OrderEntity UpdateOrder { get; set; } = new();

        public async Task OnGetAsync()
        {
            Orders = await _repository.GetAll();
        }

        public async Task OnGetSearchById(Guid id)
        {
            var order = await _repository.GetById(id);
            Orders = order != null ? new List<OrderEntity> { order } : new List<OrderEntity>();
        }

        public async Task Add(Guid id, DateTime date, bool status, Guid customerId)
        {

        }
    }
}
