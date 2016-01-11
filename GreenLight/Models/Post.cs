using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using GreenLight.Models;
using System.Text.RegularExpressions;

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
        public string TitleShort { get { return this.Title==null? "" : this.Title.Length < 15 ? this.Title : this.Title.Substring(0, 15) + "..."; } }
        public string Description { get; set; }
        public string DescriptionShort
        {
            get
            {
                var nohtml = Regex.Replace(this.Description, @"(<[^>]*>)|(\n)|(\r)|(\t)", String.Empty);
                    //.Replace("\n", "")
                    //.Replace("\r", "")
                    //.Replace("\t", "");
                return nohtml.Length < 80 
                    ? nohtml
                    : nohtml.Substring(0, 80) + "...";
            }
        }
        public DateTime CreatedOn { get; set; }

        public string PostedById { get; set; }
        [ForeignKey("PostedById")]
        public virtual ApplicationUser PostedBy { get; set; }

        public int Views { get; set; }
        public bool? Result { get; set; }

        [InverseProperty("Post")]
        public virtual List<Comment> Comments { get; set; }

        [InverseProperty("Post")]
        public virtual List<Vote> Votes { get; set; }
    }
}