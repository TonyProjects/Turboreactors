using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BPMLab.Turboreactors.DAL;
using BPMLab.Turboreactors.Models;

namespace BPMLab.Turboreactors.Controllers
{
    public class TurboreactorTypesController : Controller
    {
        private TurboreactorsContext db = new TurboreactorsContext();

        // GET: TurboreactorTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.TurboreactorTypes.ToListAsync());
        }

        // GET: TurboreactorTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TurboreactorType turboreactorType = await db.TurboreactorTypes.FindAsync(id);
            if (turboreactorType == null)
            {
                return HttpNotFound();
            }
            return View(turboreactorType);
        }

        // GET: TurboreactorTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TurboreactorTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,Description")] TurboreactorType turboreactorType)
        {
            if (ModelState.IsValid)
            {
                db.TurboreactorTypes.Add(turboreactorType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(turboreactorType);
        }

        // GET: TurboreactorTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TurboreactorType turboreactorType = await db.TurboreactorTypes.FindAsync(id);
            if (turboreactorType == null)
            {
                return HttpNotFound();
            }
            return View(turboreactorType);
        }

        // POST: TurboreactorTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,Description")] TurboreactorType turboreactorType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(turboreactorType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(turboreactorType);
        }

        // GET: TurboreactorTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TurboreactorType turboreactorType =
                await db.TurboreactorTypes
                    .Include(tt => tt.Turboreactors)
                    .SingleOrDefaultAsync(tt => tt.ID == id);
            if (turboreactorType == null)
            {
                return HttpNotFound();
            }
            if (turboreactorType.Turboreactors.Count != 0)
            {
                ViewBag.WarningMessage = "There is turboreactors related to the type";
            }
            return View(turboreactorType);
        }

        // POST: TurboreactorTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TurboreactorType turboreactorType = await db.TurboreactorTypes.FindAsync(id);
            db.TurboreactorTypes.Remove(turboreactorType);
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
