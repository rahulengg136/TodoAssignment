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

}
