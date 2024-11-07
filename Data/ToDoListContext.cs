using Microsoft.EntityFrameworkCore;

public class ToDoListContext : DbContext
{
    public ToDoListContext(DbContextOptions<ToDoListContext> options) : base(options) { }

    public DbSet<MyTask> Tasks { get; set; }
}