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
            ViewBag.Message = "Your application description page.";
            var posts = unitOfWork.Repository<Post>();
            return View(posts);
        }

        public ActionResult Ranking()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}