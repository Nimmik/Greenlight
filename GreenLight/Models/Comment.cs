﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using GreenLight.Models;

namespace GreenLight.Models
{
    public class Comment
    {
        public Comment(int postId)
        {
            this.PostId = postId;
            this.Writing = string.Empty;
        }
        public Comment()
        {
        }

        [Key]
        public int Id { get; set; }

        public string CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual ApplicationUser CreatedBy { get; set; }

        public string Writing { get; set; }

        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        public DateTime CreatedOn { get; set; }

        [InverseProperty("LikedComment")]
        public virtual List<UserCommentLike> Likes { get; set; }
    }
}