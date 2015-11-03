using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Greenlight.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        [InverseProperty("Post")]
        public List<Post> Posts { get; set; }

        [InverseProperty("Comment")]
        public List<Comment> Comments { get; set; }

        [InverseProperty("Vote")]
        public List<Vote> Votes { get; set; }
    }
}