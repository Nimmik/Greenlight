using GreenLight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JBWebappLibrary.Repository;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using GreenLight.Models.Context;

namespace GreenLight.Controllers
{
    public class BaseController : Controller
    {
        protected IUnitOfWork unitOfWork;
        protected ApplicationSignInManager _signInManager;
        protected ApplicationUserManager _userManager;


        public BaseController()
        {
            unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        public BaseController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}