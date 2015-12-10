using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using GreenLight.Models;

namespace GreenLight.Models
{
    public class Vote
    {
        public Vote(int postid)
        {
            this.PostId = postid;
        }

        public Vote()
        {

        }

        [Key]
        public int Id { get; set; }

        public string VoterId { get; set; }
        [ForeignKey("VoterId")]
        public virtual ApplicationUser Voter { get; set; }

        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        public bool OnOff { get; set; }
    }
}