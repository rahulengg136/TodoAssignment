using System;
using System.ComponentModel.DataAnnotations;

namespace AdFormAssignment.DAL.Entities
{
    public class TblTodoItem
    {
        [Key]
        public int TodoItemId { get; set; }
        public string Description { get; set; }
        public int TodoListId { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int UserId { get; set; }
    }

    public class TblTodoItemExtension : TblTodoItem
    {
        public int[] LabelIds { get; set; }
    }
}
