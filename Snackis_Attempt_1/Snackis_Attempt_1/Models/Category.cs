namespace Snackis_Attempt_1.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Models.Subcategory> Subcategories { get; set; }
    }
}
