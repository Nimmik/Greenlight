using GreenLight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JBWebappLibrary.Repository;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Greenlight.Models.Context;

namespace GreenLight.Controllers
{
    public class BaseController : Controller
    {
        protected IUnitOfWork unitOfWork;

        public BaseController()
        {
            unitOfWork = new UnitOfWork(new DefaultContext());
        }
    }
}