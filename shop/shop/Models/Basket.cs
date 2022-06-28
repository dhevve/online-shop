namespace shop
{
    public class Basket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public List<Item> Items { get; set; } = new();
    }
}