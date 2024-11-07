using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class ToDoListContext : IdentityDbContext<IdentityUser>
{
    public ToDoListContext(DbContextOptions<ToDoListContext> options) : base(options) { }

    public DbSet<MyTask> Tasks { get; set; }
}