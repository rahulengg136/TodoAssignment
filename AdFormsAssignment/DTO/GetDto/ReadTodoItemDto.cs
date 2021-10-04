using System;

namespace AdFormsAssignment.DTO
{
    /// <summary>
    /// to-do item details
    /// </summary>

    public class ReadTodoItemDto: CreateTodoItemDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int TodoItemId { get; set; }
        /// <summary>
        /// List name
        /// </summary>
        public string TodoListName { get; set; }

        /// <summary>
        /// Label name
        /// </summary>
        public string LabelName { get; set; }


    }
}
