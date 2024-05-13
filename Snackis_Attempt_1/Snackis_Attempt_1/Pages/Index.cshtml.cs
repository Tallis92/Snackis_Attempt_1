using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Snackis_Attempt_1.Pages
{
    public class IndexModel : PageModel
    {


        public Areas.Identity.Data.SnackisUser MyUser { get; set; }

        public UserManager<Areas.Identity.Data.SnackisUser> _userManager { get; set; }

        public IndexModel(UserManager<Areas.Identity.Data.SnackisUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);
            int x = 0;
        }

        public void OnPost()
        {
            
        }
    }
}
