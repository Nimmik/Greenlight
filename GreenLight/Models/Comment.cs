using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Greenlight.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public User CreatedBy { get; set; }
        public string Writing { get; set; }
        public Post Post { get; set; }
        public DateTime CreateTime { get; set; }
        public int Likes { get; set; }
    }
}