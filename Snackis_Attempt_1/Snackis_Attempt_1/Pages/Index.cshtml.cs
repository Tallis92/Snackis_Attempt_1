using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Snackis_Attempt_1.Pages
{
    public class IndexModel : PageModel
    {
        public Areas.Identity.Data.SnackisUser MyUser { get; set; }
        public string ThreadCreator { get; set; }

        public List<Models.Category> Categories { get; set; }
        [BindProperty] public Models.Category SelectedCategory { get; set; }
        public List<Models.Post> Posts { get; set; }
        public Models.Post SelectedPost { get; set; }

		public UserManager<Areas.Identity.Data.SnackisUser> _userManager { get; set; }
      
		private readonly Snackis_Attempt_1.Data.SnackisContext _context;

		public IndexModel(Snackis_Attempt_1.Data.SnackisContext context, UserManager<Areas.Identity.Data.SnackisUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int categoryId, int deleteId)
        {

            if(categoryId != 0)
            {
                SelectedCategory =  _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
            }
            if(deleteId != 0)
            {

                SelectedPost = _context.Posts.Where(p => p.Id == deleteId).SingleOrDefault();
                foreach(var comment in _context.Comments.Where(C => C.PostId == SelectedPost.Id)) 
                {
                    _context.Comments.Remove(comment);
                }
                _context.Posts.Remove(SelectedPost);
                _context.SaveChanges();
            }

            Categories = await _context.Categories.ToListAsync();
            MyUser = await _userManager.GetUserAsync(User);
            Posts = await _context.Posts.ToListAsync();

            return Page();
        }

        public void OnPost()
        {
            
        }
    }
}
