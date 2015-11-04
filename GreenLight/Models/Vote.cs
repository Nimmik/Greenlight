using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using GreenLight.Models;

namespace Greenlight.Models
{
    public class Vote
    {
        public Vote()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        [Key]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string VoterId { get; set; }
        [ForeignKey("VoterId")]
        public ApplicationUser Voter { get; set; }

        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }

        public bool OnOff { get; set; }
    }
}