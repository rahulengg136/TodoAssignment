using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.DTO.PostDto;
using System.Collections.Generic;

namespace AdFormsAssignment.DTO.GetDto
{
    /// <summary>
    /// to-do item details
    /// </summary>
    public class ReadTodoItemDto : ToDoItemCommonProperties
    {
        /// <summary>
        /// Id
        /// </summary>
        public int TodoItemId { get; set; }
        /// <summary>
        /// List name
        /// </summary>
        public string ListName { get; set; }
        /// <summary>
        /// Label name
        /// </summary>
        public List<TblLabel> Labels { get; set; }
    }
}
