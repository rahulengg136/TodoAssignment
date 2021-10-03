using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace AdFormAssignment.DAL.Entities
{
    public class MyProjectContext : DbContext
    {
        public MyProjectContext(DbContextOptions<MyProjectContext> options) : base(options)
        {

        }
        public DbSet<tblTodoList> tblTodoList { get; set; }
        public DbSet<tblTodoItem> tblTodoItem { get; set; }
        public DbSet<tblLabel> tblLabel { get; set; }
        public DbSet<tblUser> tblUser { get; set; }
    }
}
