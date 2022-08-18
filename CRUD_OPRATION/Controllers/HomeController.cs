using CRUD_OPRATION.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUD_OPRATION.Controllers
{
    public class HomeController : Controller
    {
        DURGESHEntities1 db = new DURGESHEntities1();
        public ActionResult Index()
        {
          var Data =  db.employees.ToList();
            return View(Data);
        }
        public ActionResult Create()
        {           
            return View();
        }
        [HttpPost]
        public ActionResult Create(employee e)
        {
            if (ModelState.IsValid == true)
            {
                string FileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                string extension = Path.GetExtension(e.ImageFile.FileName);
                HttpPostedFileBase postedFile = e.ImageFile;
                int length = postedFile.ContentLength;
                if(extension.ToLower() ==".jpg" || extension.ToLower() == ".jpeg"|| extension.ToLower() == ".png")
                {
                    //if (length >10000000)
                    //{
                        FileName = FileName + extension;
                        e.ImagePath= "~/Images/"+FileName;
                        FileName = Path.Combine(Server.MapPath("~/Images/"), FileName);
                        e.ImageFile.SaveAs(FileName);
                        db.employees.Add(e);
                        int a = db.SaveChanges();
                        if (a > 0)
                        {
                            TempData["createmessage"] = "<script>alert('inserted data successfull')</script>";
                            ModelState.Clear();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["createmessage"] = "<script>alert('inserted data not successfull')</script>";
                        }
                   // }
                   // else
                   // {
                     //   TempData["sizemessage"] = "<script>alert('image should be less than 1 MB')</script>";
                   // }

                }
                else
                {
                    TempData["extensionMessage"] = "<script>alert('extension error')</script>";
                }
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            var employeerow = db.employees.Where(model => model.id == id).FirstOrDefault();
            Session["image"] = employeerow.ImagePath;
            return View(employeerow);
        }
        [HttpPost]
        public ActionResult Edit(employee e)
        {
            if (ModelState.IsValid == true)
            {
                if(e.ImageFile != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                    string extension = Path.GetExtension(e.ImageFile.FileName);
                    HttpPostedFileBase postedFile = e.ImageFile;
                    int length = postedFile.ContentLength;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (length < 1000000)
                        {
                            FileName = FileName + extension;
                            e.ImagePath = "~/Images/" + FileName;
                            FileName = Path.Combine(Server.MapPath("~/Images/"), FileName);
                            e.ImageFile.SaveAs(FileName);
                            db.Entry(e).State = EntityState.Modified;
                            int a = db.SaveChanges();
                            if (a > 0)
                            {
                                string ImagePath = Request.MapPath(Session["Image"].ToString());
                                if (System.IO.File.Exists(ImagePath))
                                {
                                    System.IO.File.Delete(ImagePath);
                                }
                                TempData["updatemessage"] = "<script>alert('updated data successfull')</script>";
                                ModelState.Clear();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["updatemessage"] = "<script>alert('updated data not successfull')</script>";
                            }
                        }
                        else
                        {
                            TempData["sizemessage"] = "<script>alert('image should be less than 1 MB')</script>";
                        }

                    }
                    else
                    {
                        TempData["extensionMessage"] = "<script>alert('extension error')</script>";
                    }
                }
                else
                {
                    e.ImagePath = Session["Image"].ToString();
                    db.Entry(e).State = EntityState.Modified;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["updatemessage"] = "<script>alert('updated data successfull')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["updatemessage"] = "<script>alert('updated data not successfull')</script>";
                    }

                }
            }
           
            return View();
        }
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                var employeerow = db.employees.Where(model => model.id == id).FirstOrDefault();
                if(employeerow != null)
                {
                    db.Entry(employeerow).State = EntityState.Deleted;
                  int a=  db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["DeletedMessage"] = "<script>alert('deleted data successfull')</script>";
                        string ImagePath = Request.MapPath(employeerow.ImagePath.ToString());
                        if (System.IO.File.Exists(ImagePath))
                        {
                            System.IO.File.Delete(ImagePath);
                        }
                    }
                    else
                    {
                        TempData["DeletedMessage"] = "<script>alert('deleted data not successfull')</script>";
                    }
                }
                
                
            }
            return RedirectToAction("Index","Home");
        }
        public ActionResult Details(int id)
        {
            var employeerow = db.employees.Where(model => model.id == id).FirstOrDefault();
            Session["Image2"] = employeerow.ImagePath.ToString();
            return View(employeerow);
        }
    }
}