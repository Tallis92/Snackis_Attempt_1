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
        public Areas.Identity.Data.SnackisUser MyUser { get; set; }
        public static Models.Post SelectedPost { get; set; }
       // public string CommentImage { get; set; }
        public static Models.Comment SelectedComment { get; set; }
        [BindProperty(SupportsGet = true)] public  Models.PrivateMessage PrivateMessage { get; set; }
        public string RecieverId { get; set; }

        public readonly Data.SnackisContext _context;
        public UserManager<Areas.Identity.Data.SnackisUser> _userManager;
        public PostDetailsModel(Data.SnackisContext context, UserManager<Areas.Identity.Data.SnackisUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(int postId, int flagId, string recieverId, int deleteCommentId)
        {
			PostComments = _context.Comments.Where(c => c.PostId == postId).ToList();
			Methods.Singleton.SetPostComments(PostComments);
			MyUser = await _userManager.GetUserAsync(User);
			SelectedPost = _context.Posts.Where(p => p.Id == postId).SingleOrDefault();

			if (recieverId != null)
            {
                RecieverId = recieverId;
            }
            if(postId == 0)
            {
                postId = Methods.Singleton.GetPostId();
            }
            else
            {
                Methods.Singleton.SetPostId(postId);
            }

            if(flagId != 0)
            {
				SelectedComment = _context.Comments.Where(p => p.Id == flagId).SingleOrDefault();
                SelectedComment.Flagged = true;

                await _context.SaveChangesAsync();
			}

            if(deleteCommentId != 0)
            {
                SelectedComment = await _context.Comments.Where(c => c.Id == deleteCommentId).SingleOrDefaultAsync();
                _context.Comments.Remove(SelectedComment);
                await _context.SaveChangesAsync();
			}
            

			return Page();
		}

        public async Task<IActionResult> OnPostAsync(int postId, string recieverId)
        {
			MyUser = await _userManager.GetUserAsync(User);

			if (CreateComment.TextContent != null)
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
		    if (PrivateMessage.Message != null || PrivateMessage.Message == "")
			{
                PrivateMessage.RecievingUserId = recieverId;
                PrivateMessage.UserId = MyUser.Id;
                PrivateMessage.SentDate = DateTime.Today.Date;
                PrivateMessage.Flagged = false;
                _context.PrivateMessages.Add(PrivateMessage);
                await _context.SaveChangesAsync();
			}
            return RedirectToPage("./Index");
        }
    }
}
