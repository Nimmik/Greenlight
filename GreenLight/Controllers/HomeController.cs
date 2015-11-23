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

        public ActionResult Detail(int id)
        {
            var post = unitOfWork.Repository<Post>().GetByID(id);
            post.Views++;
            unitOfWork.Repository<Post>().Update(post);
            unitOfWork.Save();
            return View(post);
        }

        public ActionResult Search(string query)
        {
            var userId = User.Identity.GetUserId();
            var posts = unitOfWork.Repository<Post>().Get(a => a.Title.Contains(query));
            return View("OnOrOff", posts);
        }
        
        //RankingPage
        public ActionResult Ranking()
        {
            var ranks = unitOfWork.Repository<Post>().Get();

            return View();
        }
    }
}