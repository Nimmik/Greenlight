using GreenLight.Models;
using GreenLight.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JBWebappLibrary.Repository;
using Microsoft.AspNet.Identity;
using System.Net;

namespace GreenLight.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        #region OnOrOff Page

        public ActionResult OnOrOff()
        {
            var posts = unitOfWork.Repository<Post>().Get();

            return View(posts);
        }

        public ActionResult Create()
        {
            if (User.Identity.GetUserId() == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Home/Create" });
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                if(post.Title == null)
                {
                    post.Title = "제목없음";
                }
                post.PostedById = User.Identity.GetUserId();
                post.CreatedOn = DateTime.Now;
                post.Result = null;
                post.Views = 0;
            }
            unitOfWork.Repository<Post>().Insert(post);
            unitOfWork.Save();
            return RedirectToAction("OnOrOff", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id)
        {
            var model = unitOfWork.Repository<Post>().GetByID(Id);
            if(model == null)
            {
                return new HttpNotFoundResult();
            }
            else if(User.Identity.IsAuthenticated && (User.Identity.GetUserId().Equals(model.PostedById)))
            { 
                foreach(var c in model.Comments.ToList())
                {
                    unitOfWork.Repository<Comment>().Delete(c.Id);
                }
                unitOfWork.Repository<Post>().Delete(model.Id);
                unitOfWork.Save();
            }
            return RedirectToAction("OnOrOff", "Home");
        }
        
        public ActionResult Search(string query)
        {
            var userId = User.Identity.GetUserId();
            var posts = unitOfWork.Repository<Post>().Get(a => a.Title.Contains(query));
            return View("OnOrOff", posts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateResult(int postId, bool? result)
        {
            var post = unitOfWork.Repository<Post>().GetByID(postId);
            post.Result = result;
            unitOfWork.Repository<Post>().Update(post);
            unitOfWork.Save();
            return RedirectToAction("OnOrOff", "Home");
        }

        #endregion

        #region Detail Page

        public ActionResult Detail(int id)
        {
            var post = unitOfWork.Repository<Post>().GetByID(id);
            unitOfWork.Repository<Comment>().Get();
            post.Views++;
            unitOfWork.Repository<Post>().Update(post);
            unitOfWork.Save();
            return View(post);
        }

        public ActionResult Edit(int id)
        {
            var post = unitOfWork.Repository<Post>().GetByID(id);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Post post)
        {
            unitOfWork.Repository<Post>().Update(post);
            unitOfWork.Save();
            return RedirectToAction("Detail", "Home", new { id = post.Id });
        }

        public ActionResult GetComment(int postId)
        {
            var model = getCommentViewModel(unitOfWork.Repository<Comment>().Get(a => a.PostId.Equals(postId)));
            ViewBag.postId = postId;
            return PartialView("_CommentSection", model);
        }
       
        #endregion

        #region _Comment

        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpUnauthorizedResult();
            }
            else if (ModelState.IsValid)
            {
                comment.CreatedOn = DateTime.Now;
                comment.CreatedById = User.Identity.GetUserId();
                unitOfWork.Repository<Comment>().Insert(comment);
                unitOfWork.Save();
            }
            var model = getCommentViewModel(unitOfWork.Repository<Comment>().Get(a => a.PostId.Equals(comment.PostId)));
            ViewBag.postId = comment.PostId;
            return PartialView("_CommentSection",model);
        }

        public ActionResult DeleteComment(int commentId)
        {
            var userId = User.Identity.GetUserId();
            var comment =
                unitOfWork.Repository<Comment>().Get(a => a.Id.Equals(commentId) && a.CreatedById.Equals(userId));
            if (comment.Any())
            {
                unitOfWork.Repository<Comment>().Delete(comment.First());
                unitOfWork.Save();
            }
            else if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
            var postId = comment.First().PostId;
            var model = getCommentViewModel(unitOfWork.Repository<Comment>().Get(a => a.PostId.Equals(postId)));
            ViewBag.postId = postId;
            return PartialView("_CommentSection", model);
        }

        public ActionResult EditComment(int commentid, string writing)
        {
            var comment = unitOfWork.Repository<Comment>().GetByID(commentid);
            var postid = comment.PostId;
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "Account", new { returnUrl = "/Home/Detail/" + postid });
            }            
            comment.Writing = writing;
            unitOfWork.Repository<Comment>().Update(comment);
            unitOfWork.Save();
            var model = getCommentViewModel(unitOfWork.Repository<Comment>().Get(a => a.PostId.Equals(postid)));
            ViewBag.postId = postid;
            return PartialView("_CommentSection", model);
        }

        public ActionResult UpdateLike(int commentId, bool like)
        {
            var comment = unitOfWork.Repository<Comment>().GetByID(commentId);
            var postid = comment.PostId;
            var userId = User.Identity.GetUserId();
            bool? currentUserChoice = like;
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "Account", new { returnUrl = "/Home/Detail/" + postid });
            }
            var Likes = unitOfWork.Repository<UserCommentLike>().Get(a => a.LikedCommentId.Equals(commentId) && a.LikeUserId.Equals(userId));
            if(Likes.Any())
            {
                if (Likes.First().Up.Equals(like))
                {
                    unitOfWork.Repository<UserCommentLike>().Delete(Likes.First().id);
                    currentUserChoice = null;
                }
                else
                {
                    Likes.First().Up = like;
                    unitOfWork.Repository<UserCommentLike>().Update(Likes.First());
                }
            }
            else
            {
                var likeModel = new UserCommentLike()
                {
                    LikeUserId = userId,
                    LikedCommentId = commentId,
                    Up = like
                };
                unitOfWork.Repository<UserCommentLike>().Insert(likeModel);
            }
            unitOfWork.Save();
            var comments = unitOfWork.Repository<Comment>().Get(a => a.PostId.Equals(postid));
            var model = getCommentViewModel(comments);
            ViewBag.postId = postid;
            return PartialView("_CommentSection", model);
        }
        
        #endregion

        #region _Vote

        [HttpPost]
        public ActionResult VoteFor(Vote vote)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpUnauthorizedResult();
            }
            else if (ModelState.IsValid && unitOfWork.Repository<Vote>().Count(m => m.VoterId == vote.VoterId) == 0)
            {
                vote.VoterId = User.Identity.GetUserId();
                unitOfWork.Repository<Vote>().Insert(vote);
                unitOfWork.Save();
            }
            var model = unitOfWork.Repository<Vote>().Get(a => a.PostId.Equals(vote.PostId));
            ViewBag.postId = vote.PostId;
            return PartialView("_PollSection", model);
        }
       
        #endregion

        #region RankingPage

        public ActionResult Ranking()
        {
            var users = unitOfWork.Repository<ApplicationUser>().Get();
            var model = new List<RankingViewModel>();
            foreach(var u in users)
            {
                var vote = u.Votes;//All votes of a user
                double count = vote.Count();//number of votes from that user
                //votes that user picked yes and the result is yes or no and the result is no.
                double rightVotes = vote.Count(a => a.Post.Result.Equals(a.OnOff));
                double wrongVotes = count - rightVotes;
                double votePercision = rightVotes / count * 100;
                model.Add(new RankingViewModel()
                {
                    percision = votePercision,
                    voteCount = count,
                    rightVote = rightVotes,
                    wrongVote = wrongVotes                
                });
            }
            return View(model);
        }

        #endregion

        #region functions

        private IEnumerable<ViewModels.CommentViewModel> getCommentViewModel(IEnumerable<Comment> comments)
        {
            var model = new List<CommentViewModel>();
            var userId = User.Identity.GetUserId();
            foreach (var c in comments)
            {
                var l = unitOfWork.Repository<UserCommentLike>().Count(a => a.LikedCommentId.Equals(c.Id) && a.Up);
                var d = unitOfWork.Repository<UserCommentLike>().Count(a => a.LikedCommentId.Equals(c.Id) && !a.Up);
                var cuc = unitOfWork.Repository<UserCommentLike>().Get(a => a.LikedCommentId.Equals(c.Id) && a.LikeUserId.Equals(userId));
                model.Add(new CommentViewModel()
                {
                    Comment = c,
                    Like = l,
                    DisLike = d,
                    CurrentUserLike = cuc.Any() ? cuc.First().Up : (bool?)null
                });
            }
            return model;
        }

        #endregion
    }
}