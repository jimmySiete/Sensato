using Sensato.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sensato.CodeGenerator.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public DB_GeneratorEntities db = new DB_GeneratorEntities();
    }
}