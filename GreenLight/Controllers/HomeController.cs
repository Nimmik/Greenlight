using Greenlight.Models;
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

        public ActionResult OnOrOff()
        {
            var posts = unitOfWork.Repository<Post>().Get();
            return View(posts);
        }

        public ActionResult Search(string query)
        {
            var userId = User.Identity.GetUserId();
            var posts = unitOfWork.Repository<Post>().Get(a => a.Title.Contains(query));
            return View("OnOrOff", posts);
        }

        public ActionResult Ranking()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}