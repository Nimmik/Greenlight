using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreenLight.Models
{
    public class UserCommentLike
    {
        [Key]
        public int id { get; set; }
        public bool Up { get; set; }

        public string LikeUserId { get; set; }
        [ForeignKey("LikeUserId")]
        public virtual ApplicationUser LikeUser { get; set; }

        public int LikedCommentId { get; set; }
        [ForeignKey("LikedCommentId")]
        public virtual Comment LikedComment { get; set; }
    }
}