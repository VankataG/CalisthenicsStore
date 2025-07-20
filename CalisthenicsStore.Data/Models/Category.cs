namespace CalisthenicsStore.Data.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }
        public virtual ICollection<Product> Products { get; set; }
            = new HashSet<Product>();
    }
}
