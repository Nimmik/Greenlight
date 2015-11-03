using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Greenlight.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }
        public User Voter { get; set; }
        public Post Post { get; set; }
        public bool OnOff { get; set; }
    }
}