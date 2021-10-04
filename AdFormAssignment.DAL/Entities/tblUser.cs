﻿using System.ComponentModel.DataAnnotations;

namespace AdFormAssignment.DAL.Entities
{
    public class TblUser
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
