using ImageFiligran.Data;
using ImageFiligran.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ImageFiligran.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _db;

        public HomeController()
        {
            _db = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            return View(_db.Pictures.OrderByDescending(i => i.CreatedDate).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Picture model, HttpPostedFileBase image)
        {
            try
            {
                var filigran = model.FiligranName;
                if (image != null && image.ContentLength > 0)
                {
                    //watermark codes =>
                    WebImage newImage = new WebImage(image.InputStream);
                    newImage.AddTextWatermark(filigran, "Red", 50, "Bold", "Algerian", "Center", "Middle", 20, 5);
                    newImage.Save(Server.MapPath("~/img/" + image.FileName));
                    model.ImageUrl = image.FileName;

                    _db.Pictures.Add(model);
                    _db.Entry(model).State = EntityState.Added;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Update(int? id)
        {
            var updatePhoto = _db.Pictures.Find(id);
            if (updatePhoto != null)
            {
                return View(updatePhoto);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Picture model, HttpPostedFileBase image)
        {
            try
            {
                var filigran = model.FiligranName;
                if (image != null && image.ContentLength > 0)
                {
                    WebImage newImage = new WebImage(image.InputStream);
                    newImage.AddTextWatermark(filigran, "Red", 50, "Bold", "Algerian", "Center", "Middle", 20, 5);
                    newImage.Save(Server.MapPath("~/img/"), image.FileName);
                    model.ImageUrl = image.FileName;

                    model.UpdatedDate = DateTime.Now.ToLocalTime();
                    _db.Pictures.Attach(model);
                    _db.Entry(model).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            var updatePhoto = _db.Pictures.Find(id);
            if (updatePhoto != null)
            {
                _db.Pictures.Remove(updatePhoto);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}