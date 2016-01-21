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
    }
}