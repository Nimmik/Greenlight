using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using GreenLight.Models;

namespace GreenLight.Models
{
    public class Post
    {
        public Post()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DescriptionShort { get { return this.Description.Length < 200 ? this.Description : this.Description.Substring(0, 200) + "..."; } }
        public DateTime CreatedOn { get; set; }

        public string PostedById { get; set; }
        [ForeignKey("PostedById")]
        public virtual ApplicationUser PostedBy { get; set; }

        public int Views { get; set; }
        public bool? Result { get; set; }
        
        [InverseProperty("Post")]
        public List<Comment> Comments { get; set; }
        
        [InverseProperty("Post")]
        public List<Vote> Votes { get; set; }
    }
}