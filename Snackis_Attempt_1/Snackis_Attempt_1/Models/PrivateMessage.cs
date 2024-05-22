namespace Snackis_Attempt_1.Models
{
    public class PrivateMessage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
		public virtual Areas.Identity.Data.SnackisUser? User { get; set; }
		public string? UserId { get; set; }

		public virtual Areas.Identity.Data.SnackisUser? RecievingUser { get; set; }
		public string? RecievingUserId { get; set; }

		public DateTime SentDate { get; set; }
        public bool Flagged { get; set; }

    }
}
