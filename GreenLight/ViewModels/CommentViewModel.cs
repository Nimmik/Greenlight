using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight.ViewModels
{
    public class CommentViewModel
    {
        public Models.Comment Comment { get; set; }
        public int Like { get; set; }
        public int DisLike { get; set; }
        public bool? CurrentUserLike { get; set; }
    }
}
