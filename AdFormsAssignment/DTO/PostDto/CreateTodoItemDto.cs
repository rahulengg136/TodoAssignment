using System;

namespace AdFormsAssignment.DTO
{
    /// <summary>
    /// to-do item details
    /// </summary>
    public class CreateTodoItemDto
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
        /// <summary>
        /// LabelId
        /// </summary>
        public int LabelId { get; set; }
     
    }
    /// <summary>
    /// DTO to update a to-do item
    /// </summary>
    public class UpdateTodoItemDto:CreateTodoItemDto
    {
        /// <summary>
        /// To-do item unique id
        /// </summary>
        public int TodoItemId { get; set; }
    }

}
