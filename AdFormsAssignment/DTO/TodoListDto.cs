using System;

namespace AdFormsAssignment.DTO
{
    public class TodoListDto
    {
        public int TodoListId { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int LabelId { get; set; }
        public string ListName { get; set; }
        public int UserId { get; set; }

    }
}
