using AdFormAssignment.DAL.Entities;
using System;

namespace AdFormsAssignment.DTO
{
    /// <summary>
    /// to-do item details
    /// </summary>

    public class TodoItemDetail: TblTodoItem
    {
        /// <summary>
        /// List name
        /// </summary>
        public string ListName { get; set; }

        /// <summary>
        /// Label name
        /// </summary>
        public string LabelName { get; set; }


    }
}
