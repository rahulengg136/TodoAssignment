using System;

namespace AdFormsAssignment.DTO
{
    /// <summary>
    /// To do list information
    /// </summary>
    public class TodoListDto
    {
        /// <summary>
        /// List unique id
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
        /// ListName
        /// </summary>
        public string ListName { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

    }
}
