using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Model.Configurations; 

public class ToDoDbContext : DbContext {

    public DbSet<ToDo> ToDos { get; set; } = null!;
    
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }
}