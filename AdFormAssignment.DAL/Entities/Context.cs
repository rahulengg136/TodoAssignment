using Microsoft.EntityFrameworkCore;

namespace AdFormAssignment.DAL.Entities
{
    public class MyProjectContext : DbContext
    {
        public MyProjectContext(DbContextOptions<MyProjectContext> options) : base(options)
        {

        }
        public DbSet<TblTodoList> tblTodoList { get; set; }
        public DbSet<TblTodoItem> tblTodoItem { get; set; }
        public DbSet<TblLabel> tblLabel { get; set; }
        public DbSet<TblUser> tblUser { get; set; }
    }
}
