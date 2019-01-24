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
    public class EmailsController : Controller
    {
        private ContactsEntities db = new ContactsEntities();

        // GET: Emails
        [ChildActionOnly]
        public ActionResult Index(int id)
        {
            ViewBag.ContactId = id;
            var emails = db.Emails.Where(e => e.ContactId == id);
            return PartialView("Index", emails.ToList());
        }

        // GET: Emails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Email email = db.Emails.Find(id);
            if (email == null)
            {
                return HttpNotFound();
            }
            return View(email);
        }

        // GET: Emails/Create
        public ActionResult Create(int contactId)
        {
            Email email = new Email();
            email.ContactId = contactId;
            ViewBag.ContactId = contactId;
            ViewBag.EmailTypeId = new SelectList(db.EmailTypes, "EmailTypeId", "EmailTypeName");
            return View(email);
        }

        // POST: Emails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmailId,ContactId,EmailAddress,EmailTypeId")] Email email)
        {
            if (ModelState.IsValid && VerifyEmailType(email))
            {
                db.Emails.Add(email);
                db.SaveChanges();
                return RedirectToAction("Index", "Contacts");
            }

            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", email.ContactId);
            ViewBag.EmailTypeId = new SelectList(db.EmailTypes, "EmailTypeId", "EmailTypeName", email.EmailTypeId);
            return View(email);
        }

        // GET: Emails/Edit/5
        public ActionResult Edit(int? id, int contactId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Email email = db.Emails.Find(id);
            if (email == null)
            {
                return HttpNotFound();
            }
            email.ContactId = contactId;
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", email.ContactId);
            ViewBag.EmailTypeId = new SelectList(db.EmailTypes, "EmailTypeId", "EmailTypeName", email.EmailTypeId);
            return View(email);
        }

        // POST: Emails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmailId,ContactId,EmailAddress,EmailTypeId")] Email email)
        {
            if (ModelState.IsValid && VerifyEmailType(email))
            {
                db.Entry(email).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Contacts");
            }
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", email.ContactId);
            ViewBag.EmailTypeId = new SelectList(db.EmailTypes, "EmailTypeId", "EmailTypeName", email.EmailTypeId);
            return View(email);
        }

        // GET: Emails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Email email = db.Emails.Find(id);
            if (email == null)
            {
                return HttpNotFound();
            }
            return View(email);
        }

        // POST: Emails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Email email = db.Emails.Find(id);
            db.Emails.Remove(email);
            db.SaveChanges();
            return RedirectToAction("Index", "Contacts");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool VerifyEmailType(Email email)
        {
            if (email.EmailId == 0)
            {
                if (db.Emails.ToList().Find(e => e.EmailTypeId == email.EmailTypeId && e.ContactId == email.ContactId) == null)
                    return true;
            }
            else
            {
                if (db.Emails.ToList().Find(e => 
                e.EmailTypeId == email.EmailTypeId 
                && e.ContactId == email.ContactId
                && e.EmailId != email.EmailId) == null)
                    return true;
            }

            ViewBag.Msg = "Já existe um email do mesmo tipo cadastrado para o usuário.";
            return false;
        }
    }
}
