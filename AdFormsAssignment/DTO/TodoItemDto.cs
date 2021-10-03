using System;

namespace AdFormsAssignment.DTO
{
    /// <summary>
    /// todo item details
    /// </summary>
    public class TodoItemDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int TodoId { get; set; }
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
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
    }
}
