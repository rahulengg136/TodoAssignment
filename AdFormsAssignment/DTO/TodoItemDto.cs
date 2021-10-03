using System;

namespace AdFormsAssignment.DTO
{
    public class TodoItemDto
    {
        public int TodoId { get; set; }
        public string Description { get; set; }
        public int TodoListId { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int LabelId { get; set; }
        public int UserId { get; set; }
    }
}
