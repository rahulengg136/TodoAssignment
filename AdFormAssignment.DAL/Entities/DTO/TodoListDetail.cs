using System.Collections.Generic;

namespace AdFormAssignment.DAL.Entities.DTO
{
    /// <summary>
    /// To do list information
    /// </summary>
    public class TodoListDetail : TblTodoList
    {
        /// <summary>
        /// Labels info
        /// </summary>
        public List<TblLabel> Labels { get; set; }
    }
}
