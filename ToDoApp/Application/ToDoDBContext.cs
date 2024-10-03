using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain;

namespace ToDoApp.Application
{
    public class ToDoDBContext : DbContext
    {
        public ToDoDBContext(DbContextOptions<ToDoDBContext> options) : base(options) { }
        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
