using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System;

namespace Snackis_Attempt_1.Pages.RoleAdmin
{
    public class CategoryAdminModel : PageModel
    {
		internal List<Models.Category> Categories { get; set; }
		[BindProperty] public Models.Category CreateCategory {  get; set; }
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
				try
				{
					var deleteCategory = await _context.Categories.Where(c => c.Id == deleteId).FirstOrDefaultAsync();
					var deletePosts = await _context.Posts.Where(p => p.CategoryId == deleteCategory.Id).ToListAsync();
					if (deleteCategory != null || deletePosts != null)
					{						
						foreach (var post in deletePosts)
						{
							foreach (var comment in _context.Comments.Where(c => c.PostId == post.Id))
							{
								_context.Comments.Remove(comment);
							}
							_context.Posts.Remove(post);
						}
					}
					_context.Categories.Remove(deleteCategory);
					await _context.SaveChangesAsync();
				}
				catch(Exception ex)
				{

				}							
			}
			CategoryId = categoryId;
			Users = _userManager.Users.ToList();
            Categories = await DAL.CategoryManagerAPI.GetAllCategories();
            return Page();
		}

		public async Task<IActionResult> OnPostAsync(int categoryId)
		{
			var category = SelectedCategory;
			MyUser = await _userManager.GetUserAsync(User);
			if (categoryId == 0)
			{

				//CreateCategory.User = MyUser;
				CreateCategory.UserId = MyUser.Id;
				

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
