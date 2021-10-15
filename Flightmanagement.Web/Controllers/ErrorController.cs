using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flightmanagement.Web.Controllers
{
    public class ErrorController : Controller
    {
        //Controller for Error
        public ViewResult Index()
        {
            return View("Error");
        }
    }
}
