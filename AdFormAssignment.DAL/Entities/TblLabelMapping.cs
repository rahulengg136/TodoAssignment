using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdFormAssignment.DAL.Entities
{
    public class TblLabelMapping
    {
        [Key]
        public int LabelMappingId { get; set; }
        public int TodoTypeId { get; set; }
        public int RecordId { get; set; }
        public int LabelId { get; set; }
    }
}
