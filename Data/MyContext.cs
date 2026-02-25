using Microsoft.EntityFrameworkCore;
using mvc_app.Models;

namespace mvc_app.Data
{
    public class MyContext : DbContext
    {

        // napisali smo konstruktor koji prima DbContextOptions i prosljeduje ih baznoj klasi DbContext koju smo naslijedili
        public MyContext(DbContextOptions<MyContext> options) : base(options){ }

        public DbSet<Item> Items { get; set; }
}
}