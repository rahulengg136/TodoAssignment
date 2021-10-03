using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
