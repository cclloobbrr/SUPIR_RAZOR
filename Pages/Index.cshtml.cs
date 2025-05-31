using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SUPIR_RAZOR.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string Message { get; set; } = "";
        public void OnGet()
        {
            Message = "Что это?";
        }

        public void OnPost(string username)
        {
            Message = $"это {username}";
        }
    }
}
