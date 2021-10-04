using System;

namespace AdFormsAssignment.DTO
{
    /// <summary>
    /// To do list information
    /// </summary>

    public class ReadTodoListDto: CreateTodoListDto
    {
        /// <summary>
        /// List unique id
        /// </summary>
        public int TodoListId { get; set; }
        /// <summary>
        /// Label Name
        /// </summary>
        public string LabelName { get; set; }
       
    }
}
