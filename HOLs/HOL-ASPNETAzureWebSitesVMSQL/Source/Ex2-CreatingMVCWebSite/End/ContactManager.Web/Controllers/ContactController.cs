using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactManager.Web.Models;

namespace ContactManager.Web.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
        public ActionResult Index()
        {
            return this.RedirectToActionPermanent("List");
        }


        // GET: /Contact/List
        public ActionResult List(string searchQuery)
        {
            IEnumerable<Contact> contacts;

            using (ContactContext context = new ContactContext())
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    contacts = context.SearchContacts(searchQuery).ToList();
                }
                else
                {
                    contacts = context.Contacts.ToList();
                }
            }

            return View(contacts);
        }

        // GET: /Contact/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Contact/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }

            using (ContactContext context = new ContactContext())
            {
                context.Contacts.Add(contact);

                context.SaveChanges();
            }

            return this.RedirectToAction("List");
        }
    }
}
