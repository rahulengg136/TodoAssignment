namespace AdFormsAssignment.DTO
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
