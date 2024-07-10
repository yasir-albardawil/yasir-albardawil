using PlateWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace PlateWebAPI.DBContext
{
    public class PlateDbContext : DbContext
    {
        public PlateDbContext(DbContextOptions<PlateDbContext> options) : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasData(
                    new Item
                    {
                        Id = 1,
                        Name = "Apple Item",
                        ShortDescription = "Delicious apple pie",
                        LongDescription = "A classic dessert made with fresh apples and cinnamon, baked to perfection.",
                        AllergyInformation = "Contains gluten",
                        Price = 12.99M,
                        ImageUrl = "https://example.com/apple-pie.jpg",
                        ImageThumbnailUrl = "https://example.com/apple-pie-thumb.jpg",
                        IsPieOfTheWeek = true,
                        InStock = true,
                        CategoryId = 1
                    },
                    new Item
                    {
                        Id = 2,
                        Name = "Chocolate Cake",
                        ShortDescription = "Rich chocolate cake",
                        LongDescription = "Decadent chocolate cake layered with chocolate ganache and topped with chocolate shavings.",
                        AllergyInformation = "Contains dairy, gluten",
                        Price = 19.99M,
                        ImageUrl = "https://example.com/chocolate-cake.jpg",
                        ImageThumbnailUrl = "https://example.com/chocolate-cake-thumb.jpg",
                        IsPieOfTheWeek = false,
                        InStock = true,
                        CategoryId = 2,
                    });

            modelBuilder.Entity<Category>()
                .HasData(
                    new Category
                    {
                        CategoryId = 1,
                        CategoryName = "Fruit Pies",
                        Description = "Pies made with various fruits"
                    },
                    new Category
                    {
                        CategoryId = 2,
                        CategoryName = "Cakes",
                        Description = "Various types of cakes"
                    });

            base.OnModelCreating(modelBuilder);
        }
    }
}