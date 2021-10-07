using AdFormsAssignment.DTO.PostDto;

namespace AdFormsAssignment.DTO.GetDto
{
    /// <summary>
    /// Label details 
    /// </summary>
    public class ReadLabelDto : CreateLabelDto
    {
        /// <summary>
        /// Unique id of the label
        /// </summary>
        public int LabelId { get; set; }
    }
}
