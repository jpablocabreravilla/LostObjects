using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LostObjects.Backend.Models;
using LostObjects.Common.Models;
using LostObjects.Backend.Helpers;

namespace LostObjects.Backend.Controllers
{
    [Authorize]
    public class ObjecttsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Objectts
        public async Task<ActionResult> Index()
        {
            return View(await this.db.Objectts.OrderBy(o => o.PublishOn).ToListAsync());
        }

        // GET: Objectts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Objectt objectt = await this.db.Objectts.FindAsync(id);
            if (objectt == null)
            {
                return HttpNotFound();
            }
            return View(objectt);
        }

        // GET: Objectts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Objectts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( ObjectView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Objects";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }
                var objectt = this.ToObjectt(view, pic);
                this.db.Objectts.Add(objectt);
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        private Objectt ToObjectt(ObjectView view, string pic)
        {
            return new Objectt
            {
                Name = view.Name,
                ImagePath = pic,
                IsDelivered = view.IsDelivered,
                Type = view.Type,
                ObjectId = view.ObjectId,
                PublishOn = view.PublishOn,
                Description = view.Description,
                Location = view.Location,
                PhoneContact = view.PhoneContact,
            };
        }

        // GET: Objectts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var objectt = await this.db.Objectts.FindAsync(id);
            if (objectt == null)
            {
                return HttpNotFound();
            }
            return View(objectt);
        }

        // POST: Objectts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Objectt objectt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(objectt).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(objectt);
        }

        // GET: Objectts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var objectt = await this.db.Objectts.FindAsync(id);
            if (objectt == null)
            {
                return HttpNotFound();
            }
            return View(objectt);
        }

        // POST: Objectts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var objectt = await this.db.Objectts.FindAsync(id);
            this.db.Objectts.Remove(objectt);
            await db.SaveChangesAsync();
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
    }
}
