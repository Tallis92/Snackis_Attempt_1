using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Snackis_Attempt_1.Pages
{
    public class IndexModel : PageModel
    {
        public Areas.Identity.Data.SnackisUser MyUser { get; set; }

		public List<Models.Category> Categories { get; set; }

		public UserManager<Areas.Identity.Data.SnackisUser> _userManager { get; set; }
      
		private readonly Snackis_Attempt_1.Data.SnackisContext _context;

		public IndexModel(Snackis_Attempt_1.Data.SnackisContext context, UserManager<Areas.Identity.Data.SnackisUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            
            Categories = await _context.Categories.ToListAsync();
            MyUser = await _userManager.GetUserAsync(User);
        }

        public void OnPost()
        {
            
        }
    }
}
