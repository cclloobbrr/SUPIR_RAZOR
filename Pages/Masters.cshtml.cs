using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SUPIR_RAZOR.Data.Repositories;
using SUPIR_RAZOR.Models.Entities;
using static SUPIR_RAZOR.Data.Repositories.MastersRepository;

namespace SUPIR_RAZOR.Pages
{
    public class MastersModel : PageModel
    {
        private readonly MastersRepository _repository;

        public MastersModel(MastersRepository repository)
        {
            _repository = repository;
        }
        
        [BindProperty]
        public List<MasterEntity> Masters { get; set; } = new();

        [BindProperty]
        public MasterEntity NewMaster { get; set; } = new();

        [BindProperty]
        public MasterEntity UpdateMaster { get; set; } = new();

        public async Task OnGetAsync()
        {
            Masters = await _repository.GetAll();
        }

        public async Task OnGetSortByNameASCAsync()
        {
            Masters = await _repository.GetAllNameSortASC();
        }

        public async Task OnGetSortByNameDESCAsync()
        {
            Masters = await _repository.GetAllNameSortDESC();
        }

        public async Task OnGetSortByPhoneASCAsync()
        {
            Masters = await _repository.GetAllPhoneNumberSortASC();
        }

        public async Task OnGetSortByPhoneDESCAsync()
        {
            Masters = await _repository.GetAllPhoneNumberSortDESC();
        }

        public async Task OnGetSearchByIdAsync(Guid id)
        {
            var master = await _repository.GetById(id);
            Masters = master != null ? new List<MasterEntity> { master } : new List<MasterEntity>();
        }

        public async Task OnGetSearchByNameAsync(string name)
        {
            Masters = await _repository.GetByName(name);
        }

        public async Task OnGetSearchByPhoneAsync(string phone)
        {
            Masters = await _repository.GetByPhoneNumber(phone);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var result = await _repository.Delete(id);

            switch (result)
            {
                case DeleteResult.HasDependencies:
                    TempData["ErrorMessage"] = "Cannot delete master with existing supplies";
                    break;
                case DeleteResult.NotFound:
                    TempData["ErrorMessage"] = "Master not found";
                    break;
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Masters = await _repository.GetAll();
                return RedirectToPage();
            }

            await _repository.Add(Guid.NewGuid(), NewMaster.Name, NewMaster.PhoneNumber);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync(Guid id)
        {
            var existingMaster = await _repository.GetById(id);
            if(existingMaster == null)
            {
                ModelState.AddModelError("", "Master not found");
                return RedirectToPage();
            }

            if (!string.IsNullOrEmpty(UpdateMaster.Name))
            {
                existingMaster.Name = UpdateMaster.Name;
            }

            if (!string.IsNullOrEmpty(UpdateMaster.PhoneNumber))
            {
                existingMaster.PhoneNumber = UpdateMaster.PhoneNumber;
            }

            await _repository.Update(existingMaster);
            return RedirectToPage();
        }
    }
}