using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Greenlight.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public int Views { get; set; }
        public bool? Result { get; set; }
        
        [InverseProperty("Comment")]
        public List<Comment> Comments { get; set; }
        
        [InverseProperty("Vote")]
        public List<Vote> Votes { get; set; }
    }
}