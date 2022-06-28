using Microsoft.EntityFrameworkCore;

namespace shop
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<ItemShop> ItemsShop { get; set; } = null!;
        public DbSet<Basket> Baskets { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
