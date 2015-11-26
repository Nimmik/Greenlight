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
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                foreach(var c in model.Comments)
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
        public ActionResult AddComment(Comment comment)
        {
            if (ModelState.IsValid)
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
            else
            {
                return new HttpUnauthorizedResult();
            }
            var postId = comment.First().PostId;
            var model = unitOfWork.Repository<Comment>().Get(a => a.PostId.Equals(postId));
            ViewBag.postId = postId;
            return PartialView("_CommentSection", model);
        }

        //RankingPage
        public ActionResult Ranking()
        {
            var ranks = unitOfWork.Repository<Post>().Get();

            return View();
        }
    }
}