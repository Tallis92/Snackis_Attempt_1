using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Snackis_Attempt_1.Pages
{
    public class PostDetailsModel : PageModel
    {
        public List<Models.Comment> PostComments { get; set; }
        [BindProperty(SupportsGet = true)] public Models.Comment CreateComment { get; set; }
        public static Areas.Identity.Data.SnackisUser MyUser { get; set; }
        public static Models.Post SelectedPost { get; set; }

        private readonly Data.SnackisContext _context;
        private static UserManager<Areas.Identity.Data.SnackisUser> _userManager;
        public PostDetailsModel(Data.SnackisContext context, UserManager<Areas.Identity.Data.SnackisUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(int postId)
        {
            if(postId == 0)
            {
                postId = Methods.Singleton.GetPostId();
            }
            else
            {
                Methods.Singleton.SetPostId(postId);
            }
            PostComments = _context.Comments.Where(c => c.PostId == postId).ToList();
            Methods.Singleton.SetPostComments(PostComments);
			MyUser = await _userManager.GetUserAsync(User);
            SelectedPost = _context.Posts.Where(p => p.Id == postId).SingleOrDefault();

			return Page();
		}

        public async Task<IActionResult> OnPostAsync(int postId)
        {

			if (CreateComment.TextContent != null || CreateComment.TextContent != "")
            {
				if (postId == 0)
				{
					postId = Methods.Singleton.GetPostId();
				}
				SelectedPost = _context.Posts.Where(p => p.Id == postId).SingleOrDefault();
                CreateComment.UserId = MyUser.Id;
                CreateComment.CommentCreator = MyUser.NickName;
                CreateComment.PostId = SelectedPost.Id;               
				CreateComment.PublishedDate = DateTime.Today.Date;
                CreateComment.Flagged = false;
                if(MyUser.ProfilePic == null)
                {
                    MyUser.ProfilePic = "defaultpf.jpg";
                }
                CreateComment.ProfilePic = MyUser.ProfilePic;
                _context.Comments.Add(CreateComment);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("PostDetails");
        }
    }
}
