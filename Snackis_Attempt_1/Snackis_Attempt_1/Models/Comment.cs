namespace Snackis_Attempt_1.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? TextContent { get; set; }

       //ublic virtual Post? Post { get; set; }
        public int? PostId { get; set; }
        public string CommentCreator {  get; set; }
        //blic virtual Areas.Identity.Data.SnackisUser? User { get; set; }
        public string? UserId { get; set; }

        public string? ProfilePic { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool Flagged { get; set; }
    }
}
