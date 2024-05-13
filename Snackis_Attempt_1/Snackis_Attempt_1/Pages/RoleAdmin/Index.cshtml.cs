using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Snackis_Attempt_1.Pages.RoleAdmin
{
    public class IndexModel : PageModel
    {
        public List<Areas.Identity.Data.SnackisUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
        [BindProperty(SupportsGet = true)] public string RoleName { get; set; }
        [BindProperty(SupportsGet = true)] public string AddUserId { get; set; }
        [BindProperty(SupportsGet = true)] public string RemoveUserId { get; set; }

        public readonly UserManager<Areas.Identity.Data.SnackisUser> _userManager;
        public RoleManager<IdentityRole> _roleManager { get; set; }

        public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<Areas.Identity.Data.SnackisUser> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task OnGetAsync()
        {
            Users = _userManager.Users.ToList();
			Roles = _roleManager.Roles.OrderByDescending(x => x.Id).ToList();

			if (AddUserId != null)
			{
				var alterUser = await _userManager.FindByIdAsync(AddUserId);

				await _userManager.AddToRoleAsync(alterUser, RoleName);
			}
			if (RemoveUserId != null)
			{
				var alterUser = await _userManager.FindByIdAsync(RemoveUserId);

				await _userManager.RemoveFromRoleAsync(alterUser, RoleName);
			}
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (RoleName != null)
			{
				await CreateRole(RoleName);
			}

			return RedirectToPage("./Index");
		}

		public async Task CreateRole(string roleName)
		{
			bool roleExist = await _roleManager.RoleExistsAsync(roleName);

			if (!roleExist)
			{
				var role = new IdentityRole
				{
					Name = roleName,
				};

				await _roleManager.CreateAsync(role);
				//UpdateAsync(role) för att ändra/uppdatera en befintlig post

			}
		}
	}
}
