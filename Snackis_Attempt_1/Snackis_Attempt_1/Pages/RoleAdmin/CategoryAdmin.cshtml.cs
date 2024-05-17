using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Snackis_Attempt_1.Pages.RoleAdmin
{
    public class CategoryAdminModel : PageModel
    {
		internal List<Models.Category> Categories { get; set; }
		[BindProperty] public Models.Category CreateCategory {  get; set; }
		[BindProperty] public Models.Category UpdateCategory { get; set; }
		public Areas.Identity.Data.SnackisUser MyUser { get; set; }
		public List<Areas.Identity.Data.SnackisUser> Users { get; set; }
		public int CategoryId { get; set; }


		private readonly Snackis_Attempt_1.Data.SnackisContext _context;
		private UserManager<Areas.Identity.Data.SnackisUser> _userManager;
		public CategoryAdminModel(Snackis_Attempt_1.Data.SnackisContext context, UserManager<Areas.Identity.Data.SnackisUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
		public async Task OnGetAsync(int categoryId)
		{
			CategoryId = categoryId;
			Users = _userManager.Users.ToList();
			Categories = _context.Categories.ToList();
		}

		public async Task<IActionResult> OnPostAsync(int categoryId)
		{
			CategoryId = categoryId;
			MyUser = await _userManager.GetUserAsync(User);
			if (CategoryId == 0)
			{
				
				CreateCategory.UserId = MyUser.Id;
				CreateCategory.User = MyUser;

				_context.Categories.Add(CreateCategory);
				await _context.SaveChangesAsync();
				
			}
			else
			{
				//Den skapar nog ett nytt objekt, försök få till att ändra istället
				//Kolla Webbshoppen ProductManagerAPI och ProductController från API Demo
				string text = UpdateCategory.Name;
				UpdateCategory.UserId = MyUser.Id;
				_context.Categories.Attach(UpdateCategory);
				await _context.SaveChangesAsync();
			}
			return RedirectToPage("./CategoryAdmin");
		}
	}
}
