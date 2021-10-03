using System.ComponentModel.DataAnnotations;

namespace AdFormAssignment.DAL.Entities
{
    public class tblLabel
    {
        [Key]
        public int LabelId { get; set; }
        public string LabelName { get; set; }
    }
}
