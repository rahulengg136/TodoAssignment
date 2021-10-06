using AdFormAssignment.DAL.Entities;
using System.Collections.Generic;

namespace AdFormsAssignment.DTO
{
    /// <summary>
    /// To do list information
    /// </summary>
    public class TodoListDetail : TblTodoList
    {
        /// <summary>
        /// Label Name
        /// </summary>
        public List<TblLabel> Labels { get; set; }
    }
}
