using System.Collections.Generic;

namespace AdFormAssignment.DAL.Entities.DTO
{
    /// <summary>
    /// to-do item details
    /// </summary>
    public class TodoItemDetail : TblTodoItem
    {
        /// <summary>
        /// List name
        /// </summary>
        public string ListName { get; set; }

        /// <summary>
        /// Label name
        /// </summary>
        public List<TblLabel> Labels { get; set; }
    }
}
