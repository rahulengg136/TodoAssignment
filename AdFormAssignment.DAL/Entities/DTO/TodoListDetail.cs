using AdFormAssignment.DAL.Entities;
using System;

namespace AdFormsAssignment.DTO
{
    /// <summary>
    /// To do list information
    /// </summary>

    public class TodoListDetail: TblTodoList
    {
      
        /// <summary>
        /// Label Name
        /// </summary>
        public string LabelName { get; set; }
       
    }
}
