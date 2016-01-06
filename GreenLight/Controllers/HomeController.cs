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

        //OnOrOff Page
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
                post.PostedById = User.Identity.GetUserId();
                post.CreatedOn = DateTime.Now;
                post.Result = null;
                post.Views = 0;
            }
            unitOfWork.Repository<Post>().Insert(post);
            unitOfWork.Save();
            return RedirectToAction("OnOrOff", "Home");
        }

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

        public ActionResult UpdateResult(int postId, bool? result)
        {
            var post = unitOfWork.Repository<Post>().GetByID(postId);
            post.Result = result;
            unitOfWork.Repository<Post>().Update(post);
            unitOfWork.Save();
            return RedirectToAction("OnOrOff", "Home");
        }

        //Detail Page
        public ActionResult Detail(int id)
        {
            var post = unitOfWork.Repository<Post>().GetByID(id);
            unitOfWork.Repository<Comment>().Get();
            post.Views++;
            unitOfWork.Repository<Post>().Update(post);
            unitOfWork.Save();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Post post)
        {
            
            unitOfWork.Repository<Post>().Update(post);
            unitOfWork.Save();
            return PartialView("_EditPost", post);
        }
        
        //_Comment
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
            var model = unitOfWork.Repository<Comment>().Get(a => a.PostId.Equals(comment.PostId));
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
            var model = unitOfWork.Repository<Comment>().Get(a => a.PostId.Equals(postId));
            ViewBag.postId = postId;
            return PartialView("_CommentSection", model);
        }

        //_Vote
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

        //RankingPage
        public ActionResult Ranking()
        {
            var users = unitOfWork.Repository<ApplicationUser>().Get();

            return View(users);
        }
    }
}