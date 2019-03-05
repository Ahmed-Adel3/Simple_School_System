using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using School.Models;
using System.Web.Routing;
using System.Web.Security;
using System.Data.Entity;

namespace School.Controllers
{
    public class HomeController : Controller
    {
        SchoolEntities sc = new SchoolEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult signup(string username)
        {
            Father f = new Father();
            f.username = username;
            f.last_login = DateTime.Now;
            if (ModelState.IsValid == true)
            {
                sc.Fathers.Add(f);
                sc.SaveChanges();
                Session["user"] = username;
                Session["id"] = f.f_id;
                return RedirectToAction("StudentList","Students");
            }
            else
            {
                return null;
            }
        }

        public ActionResult signin(string username)
        {
            var user = sc.Fathers.FirstOrDefault(a => a.username == username);
            if (user != null)
            {
                Session["user"] = username;
                Session["id"] = user.f_id;
                Session["LastLogin"] = sc.Fathers.SingleOrDefault(a => a.username == username).last_login;
                sc.Fathers.Where(a => a.username == username).SingleOrDefault().last_login = DateTime.Now;
                sc.SaveChanges();
                return RedirectToAction("StudentList", "Students");
            }
            else
            {
                return null;
            }
                      
        }

        public ActionResult logout()
        {
            Session["user"] = null;
            Session["id"] = null;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Json_signup(string username)
        {
            var user = sc.Fathers.FirstOrDefault(a => a.username == username);
            return Json(user == null);
        }

        [HttpPost]
        public JsonResult Json_signin(string username)
        {
            var user = sc.Fathers.FirstOrDefault(a => a.username == username);
            return Json(user != null);
        }
    }
}