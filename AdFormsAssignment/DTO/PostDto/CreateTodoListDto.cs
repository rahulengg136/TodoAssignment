using System;

namespace AdFormsAssignment.DTO
{
    /// <summary>
    /// To do list information
    /// </summary>
    public class CreateTodoListDto
    {
      
        /// <summary>
        /// Name of the list
        /// </summary>
        public string ListName { get; set; }
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
    /// DTO to update to-list
    /// </summary>
    public class UpdateTodoListDto: CreateTodoListDto
    {
        /// <summary>
        /// To-do list unique id
        /// </summary>
        public int TodoListId { get; set; }

    }
}
