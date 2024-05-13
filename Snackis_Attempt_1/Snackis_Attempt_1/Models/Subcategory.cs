namespace Snackis_Attempt_1.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }

        public List<Models.Post> Posts { get; set; }
    }
}
