namespace Snackis_Attempt_1.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public virtual Category? Category { get; set; }
        public int? CategoryId { get; set; }

        public string PostCreator { get; set; }

        public virtual Areas.Identity.Data.SnackisUser? User { get; set; }
        public string? UserId { get; set; }

        public DateTime? PublishedDate { get; set; }
        public bool? Flagged { get; set; }


    }
}
