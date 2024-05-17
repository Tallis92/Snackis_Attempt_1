namespace Snackis_Attempt_1.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Areas.Identity.Data.SnackisUser User { get; set; }
        public string UserId { get; set; }
        List<Models.Post> Posts { get; set; }
    }
}
