using PhotoUploader_WebRole.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoUploader_WebRole.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(string partitionKey, string rowKey)
        {
            return View();
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(PhotoViewModel photoViewModel, HttpPostedFileBase file, FormCollection collection)
        {
            return View();
        }
        //
        // GET: /Home/Edit/5

        public ActionResult Edit(string partitionKey, string rowKey)
        {
            return View();
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhotoViewModel photoViewModel, FormCollection collection)
        {
            return View();
        }

        //
        // GET: /Home/Delete/5

        public ActionResult Delete(string partitionKey, string rowKey)
        {
            return View();
        }

        //
        // POST: /Home/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string partitionKey, string rowKey)
        {
            return View();
        }
    }
}
