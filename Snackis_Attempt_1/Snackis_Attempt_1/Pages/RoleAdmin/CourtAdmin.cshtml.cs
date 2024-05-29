using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Snackis_Attempt_1.Pages.RoleAdmin
{
	public class CourtAdminModel : PageModel
	{
		internal List<Models.Comment> FlaggedComments { get; set; }
		internal Models.Comment SelectedComment { get; set; }

		internal Data.SnackisContext _context { get; set; }

		public CourtAdminModel(Data.SnackisContext snackisContext)
		{
			_context = snackisContext;
		}
		public async Task<IActionResult> OnGetAsync(int flagId, int deleteId)
		{

			FlaggedComments = await _context.Comments.Where(c => c.Flagged == true).ToListAsync();

			if (flagId != 0)
			{

				SelectedComment = _context.Comments.Where(p => p.Id == flagId).SingleOrDefault();
				SelectedComment.Flagged = false;

				await _context.SaveChangesAsync();
			}
			if (deleteId != 0)
			{
				SelectedComment = _context.Comments.Where(p => p.Id == deleteId).SingleOrDefault();
				_context.Comments.Remove(SelectedComment);
				await _context.SaveChangesAsync();
				deleteId = 0;
			}
            FlaggedComments = await _context.Comments.Where(c => c.Flagged == true).ToListAsync();
            //Kolla med micke om det här, rätt skumt
            return Page();
		}
	}
}
