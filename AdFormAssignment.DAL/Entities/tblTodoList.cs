using System;
using System.ComponentModel.DataAnnotations;


namespace AdFormAssignment.DAL.Entities
{
    public class tblTodoList
    {
        [Key]
        public int TodoListId { get; set; }
        public string ListName { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int LabelId { get; set; }
        public int UserId { get; set; }
    }
}
