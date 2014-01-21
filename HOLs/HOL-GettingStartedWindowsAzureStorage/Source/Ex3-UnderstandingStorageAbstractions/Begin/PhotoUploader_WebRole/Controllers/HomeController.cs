using PhotoUploader_WebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoUploader_WebRole.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string partitionKey, string rowKey)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PhotoViewModel photoViewModel, HttpPostedFileBase file, FormCollection collection)
        {
            return View();
        }

        public ActionResult Edit(string partitionKey, string rowKey)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhotoViewModel photoViewModel, FormCollection collection)
        {
            return View();
        }

        public ActionResult Delete(string partitionKey, string rowKey)
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string partitionKey, string rowKey)
        {
            return View();
        }
    }
}