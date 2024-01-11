using Model.Configurations;
using Model.Entities;

namespace Domain.Repositories.Implementations; 

public class ToDoRepository : ARepository<ToDo> {
    public ToDoRepository(ToDoDbContext context) : base(context) { }
}