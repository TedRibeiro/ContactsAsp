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
    public class PhonesController : Controller
    {
        private ContactsEntities db = new ContactsEntities();

        // GET: Phones
        [ChildActionOnly]
        public ActionResult Index(int id)
        {
            ViewBag.ContactId = id;
            var phones = db.Phones.Where(p => p.ContactId == id);
            return PartialView("Index", phones.ToList());
        }

        // GET: Phones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phone phone = db.Phones.Find(id);
            if (phone == null)
            {
                return HttpNotFound();
            }
            return View(phone);
        }

        // GET: Phones/Create
        public ActionResult Create(int id)
        {
            Phone phone = new Phone();
            phone.ContactId = id;
            ViewBag.ContactId = id;
            ViewBag.PhoneTypeId = new SelectList(db.PhoneTypes, "PhoneTypeId", "PhoneTypeName");

            return View("Create", phone);
        }

        // POST: Phones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhoneId,PhoneNumber,PhoneTypeId,ContactId")] Phone phone)
        {
            if (ModelState.IsValid)
            {
                db.Phones.Add(phone);
                db.SaveChanges();
            }

            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", phone.ContactId);
            ViewBag.PhoneTypeId = new SelectList(db.PhoneTypes, "PhoneTypeId", "PhoneTypeName", phone.PhoneTypeId);
            return View("Index", "Contacts");
        }

        // GET: Phones/Edit/5
        public ActionResult Edit(int? id, int contactId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phone phone = db.Phones.Find(id);
            phone.ContactId = contactId;
            if (phone == null)
            {
                return HttpNotFound();
            }

            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", phone.ContactId);
            ViewBag.PhoneTypeId = new SelectList(db.PhoneTypes, "PhoneTypeId", "PhoneTypeName", phone.PhoneTypeId);
            return View(phone);
        }

        // POST: Phones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhoneId,PhoneNumber,PhoneTypeId,ContactId")] Phone phone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Contacts");
            }
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", phone.ContactId);
            ViewBag.PhoneTypeId = new SelectList(db.PhoneTypes, "PhoneTypeId", "PhoneTypeName", phone.PhoneTypeId);
            return View(phone);
        }

        // GET: Phones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phone phone = db.Phones.Find(id);
            if (phone == null)
            {
                return HttpNotFound();
            }
            return View(phone);
        }

        // POST: Phones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Phone phone = db.Phones.Find(id);
            db.Phones.Remove(phone);
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
    }
}
