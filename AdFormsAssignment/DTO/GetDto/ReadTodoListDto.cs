using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.DTO.PostDto;
using System.Collections.Generic;

namespace AdFormsAssignment.DTO.GetDto
{
    /// <summary>
    /// To do list information
    /// </summary>
    public class ReadTodoListDto : ToDoListCommonProperties
    {
        /// <summary>
        /// List unique id
        /// </summary>
        public int TodoListId { get; set; }
        /// <summary>
        /// Label Name
        /// </summary>
        public List<TblLabel> Labels { get; set; }
    }
}
