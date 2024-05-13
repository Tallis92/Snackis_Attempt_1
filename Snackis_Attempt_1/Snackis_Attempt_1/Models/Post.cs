namespace Snackis_Attempt_1.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual Subcategory Subcategory { get; set; }
        public int SubcategoryId { get; set; }

        public virtual Models.User User { get; set; }
        public int UserId { get; set; }

        public List<Models.Comment> Comments { get; set; }

        public DateTime PublishedDate { get; set; }
        public bool Flagged { get; set; }


    }
}
