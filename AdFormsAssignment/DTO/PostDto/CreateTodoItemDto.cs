using System;

namespace AdFormsAssignment.DTO
{
    /// <summary>
    /// To-do item common properties that must be used in any extension of this class
    /// </summary>
    public class ToDoItemCommonProperties
    {
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// List id
        /// </summary>
        public int TodoListId { get; set; }
        /// <summary>
        /// ExpectedDate
        /// </summary>
        public DateTime ExpectedDate { get; set; }
    }

    /// <summary>
    /// to-do item details
    /// </summary>
    public class CreateTodoItemDto : ToDoItemCommonProperties
    {
        /// <summary>
        /// LabelId
        /// </summary>
        public int[] LabelIds { get; set; }
    }
}
