using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Snackis_Attempt_1.Pages.RoleAdmin
{
    public class CategoryAdminModel : PageModel
    {
		internal List<Models.Category> Categories { get; set; }
	    public Models.Category CreateCategory {  get; set; }
		//[BindProperty] public string UpdateCategoryName { get; set; }
		[BindProperty] public Models.Category SelectedCategory { get; set; }
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
		public async Task<IActionResult> OnGetAsync(int categoryId, int deleteId)
		{
			if(categoryId != 0)
			{
				SelectedCategory = _context.Categories.Where(c => c.Id == categoryId).SingleOrDefault();
			}

			if(deleteId != 0)
			{
				var deleteCategory = _context.Categories.Where(c => c.Id == deleteId).FirstOrDefault();
				_context.Categories.Remove(deleteCategory);
				await _context.SaveChangesAsync();
			}
			CategoryId = categoryId;
			Users = _userManager.Users.ToList();
			Categories = _context.Categories.ToList();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int categoryId)
		{
			var category = SelectedCategory;
			MyUser = await _userManager.GetUserAsync(User);
			if (categoryId == 0)
			{	
				CreateCategory.UserId = MyUser.Id;
				CreateCategory.User = MyUser;

				_context.Categories.Add(CreateCategory);
				await _context.SaveChangesAsync();	
			}
			else
			{		
				var editCategory =  _context.Categories.Where(c => c.Id == categoryId).SingleOrDefault();
				editCategory.Name = SelectedCategory.Name;
				await _context.SaveChangesAsync();
				categoryId = 0;

            }
			return RedirectToPage("./CategoryAdmin");
		}
	}
}
