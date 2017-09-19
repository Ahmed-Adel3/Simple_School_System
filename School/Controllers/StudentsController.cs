using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using School.Models;
using System.Text.RegularExpressions;
using PagedList;
using PagedList.Mvc;

namespace School.Controllers
{
    public class StudentsController : Controller
    {
        private SchoolEntities db = new SchoolEntities();

        public ActionResult StudentList(string SearchBy , string Search , int? page)
        {
            
            if (Session["user"] != null)
            {
                
                string sessionUser = Session["user"].ToString();

                if ( SearchBy == "Gender" && Search !=string.Empty && Search !=null)
                { 
                    var students = db.Students.Where(a => a.Father.username == sessionUser).Where(a=>a.gender == Search).OrderBy(a => a.st_id);
                    return View(students.ToList().ToPagedList(page ?? 1, 3));
                }
                else if (SearchBy == "Name" && Search != string.Empty && Search != null )
                {
                    var students = db.Students.Where(a => a.Father.username == sessionUser).Where(a => a.fullname == Search).OrderBy(a => a.st_id);
                    return View(students.ToList().ToPagedList(page ?? 1, 3));
                }
                else
                {
                    var students = db.Students.Where(a => a.Father.username == sessionUser).OrderBy(a => a.st_id);
                    return View(students.ToList().ToPagedList(page ?? 1, 3));
                }
            }
            else return Json(false);
        }

        public ActionResult Create()
        {
            ViewBag.father_id = new SelectList(db.Fathers, "f_id", "username");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "st_id,fullname,gender,birth_date,phone")] Student student)
        {
            student.fullname = Remove_Successive_spaces(student.fullname);
            if (ModelState.IsValid)
            {
                int x;
                int.TryParse( Session["id"].ToString(), out  x);
                student.father_id = x;
                db.Students.Add(student);
                db.SaveChanges(); 
                return RedirectToAction("StudentList");
            }

            ViewBag.father_id = new SelectList(db.Fathers, "f_id", "username", student.father_id);
            return View(student);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            //ViewBag.father_id = new SelectList(db.Fathers, "f_id", "username", student.father_id);
            return PartialView("_EditPartial",student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "st_id,fullname,gender,birth_date,phone")] Student student)
        {
            student.fullname = Remove_Successive_spaces(student.fullname);
            if ( ModelState.IsValid)
            {
                int x;
                int.TryParse(Session["id"].ToString(), out x);
                student.father_id = x;        
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();               
            }
            return RedirectToAction("StudentList");
        }

        [HttpPost]
        public JsonResult Json_fullname(string fullname, string InitialName)
        {

           if (fullname == InitialName)
            {
                return Json(true);
            }
            else
            {
                fullname = Remove_Successive_spaces(fullname);
                var user = db.Students.FirstOrDefault(a => a.fullname == fullname);
                return Json(user == null);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("StudentList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public string Remove_Successive_spaces (string name)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            name= regex.Replace(name, " ");
            return name;
        }

    }
}
