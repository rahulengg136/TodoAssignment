using Microsoft.EntityFrameworkCore;

namespace AdFormAssignment.DAL.Entities
{
    public class MyProjectContext : DbContext
    {
        public MyProjectContext(DbContextOptions<MyProjectContext> options) : base(options) { }

        public DbSet<TblTodoList> TblTodoList { get; set; }
        public DbSet<TblTodoItem> TblTodoItem { get; set; }
        public DbSet<TblLabel> TblLabel { get; set; }
        public DbSet<TblUser> TblUser { get; set; }
        public DbSet<TblLabelMapping> TblLabelMapping { get; set; }
    }
}
