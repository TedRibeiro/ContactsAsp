using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContactsAspMvc.Models;

namespace ContactsAspMvc.Controllers
{
    public class ContactsController : Controller
    {
        private ContactsEntities db = new ContactsEntities();

        // GET: Contacts
        public ActionResult Index(string ContactName)
        {
            this.GenerateDropdown();

            if (string.IsNullOrWhiteSpace(ContactName) || string.Equals(ContactName, "Todos"))
                return View(db.Contacts.ToList());

            return View(db.Contacts.ToList().Where(c => c.ContactName.StartsWith(ContactName)));
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContactId,ContactName,ContactDateOfBirth")] Contact contact)
        {
            if (ModelState.IsValid && VerifyUserName(contact))
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContactId,ContactName,ContactDateOfBirth")] Contact contact)
        {
            if (ModelState.IsValid && VerifyUserName(contact))
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool VerifyUserName(Contact contact)
        {
            if(contact.ContactId == 0)
            {
                if (db.Contacts.ToList().Find(c => c.ContactName.Trim().Equals(contact.ContactName.Trim())) == null)
                return true;
            }
            else
            {
                if (db.Contacts.ToList().Find(c =>
                    c.ContactName.Trim().Equals(contact.ContactName.Trim())
                    && c.ContactId != contact.ContactId
                ) == null)
                    return true;
            }

            ViewBag.Msg = "Já existe um usuário cadastrado com este nome.";
            return false;
        }

        public void GenerateDropdown()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Todos", Value = "" });
            for(char l = 'A'; l <= 'Z'; l++)
                items.Add(new SelectListItem() { Text = l.ToString(), Value = l.ToString() });

            var ddl = from n in items
                      select n.Text;
            ViewBag.Letter = ddl;
        }
    }
}
