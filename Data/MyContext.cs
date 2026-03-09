using Microsoft.EntityFrameworkCore;
using mvc_app.Models;


namespace mvc_app.Data
{
    public class MyContext : DbContext
    {
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Item>().HasData(
        //         new Item {Id=411, Name="microphone", Price=6768.68, SerialNumberId=401401 }
        //     );
        //     modelBuilder.Entity<SerialNumber>().HasData(
        //         new SerialNumber {Id=401401, Name="MCH155342", ItemId=411 }
        //     );

        //     modelBuilder.Entity<Category>().HasData(
        //         new Category {Id=888, Name="PC_Component"}, 
        //         new Category {Id=111, Name="Electronics"}, 
        //         new Category {Id=222, Name="Books"} 
        //     );


        //     base.OnModelCreating(modelBuilder);
        // }

        // napisali smo konstruktor koji prima DbContextOptions i prosljeduje ih baznoj klasi DbContext koju smo naslijedili
        public MyContext(DbContextOptions<MyContext> options) : base(options){ }

        public DbSet<Item> Items { get; set; }

        public DbSet<SerialNumber> SerialNumbers { get; set; }

        public DbSet<Category> Categories {get; set;} 
}
}