using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreenLight.ViewModels
{
    public class RankingViewModel
    {
        public double percision { get; set; }
        public double voteCount { get; set; }
        public double rightVote { get; set; }
        public double wrongVote { get; set; }
    }
}