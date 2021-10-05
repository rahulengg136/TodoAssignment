using System;

namespace AdFormsAssignment.DTO
{
    /// <summary>
    /// To-Do list common properties that must be used in any extensions
    /// </summary>
    public class ToDoListCommonProperties
    {
        /// <summary>
        /// Name of the list
        /// </summary>
        public string ListName { get; set; }
        /// <summary>
        /// ExpectedDate
        /// </summary>
        public DateTime ExpectedDate { get; set; }
    }

    /// <summary>
    /// To do list information
    /// </summary>
    public class CreateTodoListDto : ToDoListCommonProperties
    {
        /// <summary>
        /// LabelId
        /// </summary>
        public int[] LabelIds { get; set; }
 
    }
  
}
