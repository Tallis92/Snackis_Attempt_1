namespace Snackis_Attempt_1.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string TextContent { get; set; }

        public virtual Post Post { get; set; }
        public int PostId { get; set; }

        public virtual Models.User User { get; set; }
        public int UserId { get; set; }

        public DateTime PublishedDate { get; set; }
        public bool Flagged { get; set; }
    }
}
