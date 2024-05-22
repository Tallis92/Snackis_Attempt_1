using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis_Attempt_1.Methods;

namespace Snackis_Attempt_1.Pages
{
    public class CreateAPostModel : PageModel
    {
        public static Areas.Identity.Data.SnackisUser MyUser { get; set; }
        [BindProperty(SupportsGet = true)] public Models.Post CreatePost { get; set; }
        public Models.Category Category { get; set; }
        public static UserManager<Areas.Identity.Data.SnackisUser> _userManager {  get; set; }
		private readonly Snackis_Attempt_1.Data.SnackisContext _context;

		public CreateAPostModel(UserManager<Areas.Identity.Data.SnackisUser> userManager, Snackis_Attempt_1.Data.SnackisContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);
        }

        public async Task<IActionResult> OnPostAsync(int categoryId)
        {
            bool success = false;
            if(categoryId != 0) 
            {
                Category = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
                CreatePost.Category = Category;
                CreatePost.CategoryId = categoryId;
                CreatePost.UserId = MyUser.Id;
                CreatePost.PostCreator = MyUser.NickName;
                CreatePost.PublishedDate = DateTime.Today.Date;
                CreatePost.Flagged = false;
                
                _context.Posts.Add(CreatePost);
                await _context.SaveChangesAsync();
                success = true;
                Methods.Singleton.SetPostId(CreatePost.Id);
            }
            if(success == true)
            {
				return RedirectToPage("/PostDetails");
			}
            else
            {
                return Page();
			}
			
		}
    }
}
