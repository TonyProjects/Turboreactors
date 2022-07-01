using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BPMLab.Turboreactors.DAL;
using BPMLab.Turboreactors.Models;
using BPMLab.Turboreactors.Services;

namespace BPMLab.Turboreactors.Controllers
{
    public class ManufacturesController : Controller
    {
        private TurboreactorsContext db = new TurboreactorsContext();
        private UploadService _uploadService = new UploadService();

        // GET: Manufactures
        public async Task<ActionResult> Index()
        {
            return View(await db.Manufactures.Include(m => m.LogoImage).ToListAsync());
        }

        // GET: Manufactures/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacture manufacture = await db.Manufactures.FindAsync(id);
            if (manufacture == null)
            {
                return HttpNotFound();
            }
            return View(manufacture);
        }

        // GET: Manufactures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manufactures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,OfficeBuildingColor")] Manufacture manufacture, 
                                               [Bind(Include = "logoImage")]HttpPostedFileBase logoImage)
        {
            if (logoImage != null)
            {
                var storedLogoImage = new StoredFile
                { 
                    Name = Guid.NewGuid().ToString() + "_" + logoImage.FileName 
                };
                manufacture.LogoImage = storedLogoImage;

                await _uploadService.UploadFileAsync(logoImage,
                                                     Server.MapPath("~/UploadedFiles"),
                                                     storedLogoImage.Name);
            }
            db.Manufactures.Add(manufacture);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Manufactures/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacture manufacture = await db.Manufactures
                                            .Include(m => m.LogoImage)
                                            .FirstOrDefaultAsync(m => m.ID == id);
            if (manufacture == null)
            {
                return HttpNotFound();
            }
            return View(manufacture);
        }

        // POST: Manufactures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,OfficeBuildingColor")] Manufacture manufacture,
                                             [Bind(Include = "logoImage")] HttpPostedFileBase logoImage)
        {
            if (ModelState.IsValid)
            {
                manufacture.LogoImage = db.Images.Add(
                   new StoredFile { Name = Guid.NewGuid().ToString() + logoImage.FileName });

                await _uploadService.UploadFileAsync(logoImage,
                                               Server.MapPath("~/UploadedFiles"),
                                               manufacture.LogoImage.Name);

                db.Entry(manufacture).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(manufacture);
        }

        // GET: Manufactures/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacture manufacture = await db.Manufactures
                .Include(m => m.Turboreactors)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (manufacture == null)
            {
                return HttpNotFound();
            }
            if (manufacture.Turboreactors.Count != 0)
            {
                ViewBag.WarningMessage = "There is turboreactors related to the manufacture";
            }
            return View(manufacture);
        }

        // POST: Manufactures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Manufacture manufacture = await db.Manufactures.FindAsync(id);
            db.Manufactures.Remove(manufacture);
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
