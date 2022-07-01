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
using BPMLab.Turboreactors.ViewModels;
using System.Data.Entity.Infrastructure;

namespace BPMLab.Turboreactors.Controllers
{
    public class TurboreactorsController : Controller
    {
        private TurboreactorsContext db = new TurboreactorsContext();

        // GET: Turboreactors
        public async Task<ActionResult> Index(string[] selectedTypes)
        {
            IQueryable<Turboreactor> turboreactors;
            if (selectedTypes != null)
            {
                turboreactors = db.Turboreactors
                    .Include(t => t.Manufacture)
                    .Where(t => t.Types.Any(tt => selectedTypes.Contains(tt.ID.ToString())));   
            }
            else
            {
                turboreactors = db.Turboreactors.Include(t => t.Manufacture);
            }
            PopulateSelectedTypeData(selectedTypes);
            return View(await turboreactors.ToListAsync());
        }

        // GET: Turboreactors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turboreactor turboreactor = await db.Turboreactors.FindAsync(id);
            if (turboreactor == null)
            {
                return HttpNotFound();
            }
            return View(turboreactor);
        }

        // GET: Turboreactors/Create
        public ActionResult Create()
        {
            var turboreactor = new Turboreactor();
            PopulateSelectedTypeData(turboreactor);
            ViewBag.ManufactureID = new SelectList(db.Manufactures, "ID", "Name");
            return View();
        }

        // POST: Turboreactors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,Power,BladesCount,StartDate,ManufactureID")] Turboreactor turboreactor, string[] selectedTypes)
        {
            if (selectedTypes != null)
            {
                turboreactor.Types = new List<TurboreactorType>();
                foreach (var type in selectedTypes)
                {
                    var typeToAdd = db.TurboreactorTypes.Find(int.Parse(type));
                    turboreactor.Types.Add(typeToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                db.Turboreactors.Add(turboreactor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            PopulateSelectedTypeData(turboreactor);
            ViewBag.ManufactureID = new SelectList(db.Manufactures, "ID", "Name", turboreactor.ManufactureID);
            return View(turboreactor);
        }

        // GET: Turboreactors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turboreactor turboreactor = await db.Turboreactors.FindAsync(id);
            if (turboreactor == null)
            {
                return HttpNotFound();
            }
            PopulateSelectedTypeData(turboreactor);
            ViewBag.ManufactureID = new SelectList(db.Manufactures, "ID", "Name", turboreactor.ManufactureID);
            return View(turboreactor);
        }

        private void PopulateSelectedTypeData(Turboreactor turboreactor)
        {
            var allTypes = db.TurboreactorTypes;
            var turboreatorTypes = new HashSet<int>(turboreactor.Types.Select(tt => tt.ID));

            var viewModel = new List<SelectedTypeData>();
            foreach (var type in allTypes)
            {
                viewModel.Add(new SelectedTypeData
                {
                    TypeID = type.ID,
                    Name = type.Name,
                    Selected = turboreatorTypes.Contains(type.ID)
                });
            }
            ViewBag.Types = viewModel;
        }

        private void PopulateSelectedTypeData(string[] selectedTypes)
        {
            var allTypes = db.TurboreactorTypes;

            var viewModel = new List<SelectedTypeData>();
            foreach (var type in allTypes)
            {
                viewModel.Add(new SelectedTypeData
                {
                    TypeID = type.ID,
                    Name = type.Name,
                    Selected = (selectedTypes?.Contains(type.ID.ToString()) ?? false)
                });
            }
            ViewBag.Types = viewModel;
        }

        // POST: Turboreactors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, string[] selectedTypes)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var turboreactorToUpdate = db.Turboreactors
                .Include(t => t.Manufacture)
                .Include(t => t.Types)
                .Where(t => t.ID == id)
                .Single();

            if (TryUpdateModel(turboreactorToUpdate, "", 
                new string[] { "Name", "Power", "BladesCount", "StartDate", "Manufacture" }))
            {
                try
                {
                    UpdateTurboreactorTypes(selectedTypes, turboreactorToUpdate);

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateSelectedTypeData(turboreactorToUpdate);
            ViewBag.ManufactureID = new SelectList(db.Manufactures, "ID", "Name", turboreactorToUpdate.ManufactureID);
            return View(turboreactorToUpdate);
        }

        private void UpdateTurboreactorTypes(string[] selectedTypes, Turboreactor turboreactorToUpdate)
        {
            if (selectedTypes == null)
            {
                turboreactorToUpdate.Types = new List<TurboreactorType>();
                return;
            }

            var selectedTypesHS = new HashSet<string>(selectedTypes);
            var turboreactorTypes = new HashSet<int> (turboreactorToUpdate.Types.Select(t => t.ID));
            foreach (var type in db.TurboreactorTypes)
            {
                if (selectedTypesHS.Contains(type.ID.ToString()))
                {
                    if (!turboreactorTypes.Contains(type.ID))
                    {
                        turboreactorToUpdate.Types.Add(type);
                    }
                }
                else
                {
                    if (turboreactorTypes.Contains(type.ID))
                    {
                        turboreactorToUpdate.Types.Remove(type);
                    }
                }
            }
        }

        // GET: Turboreactors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turboreactor turboreactor = await db.Turboreactors.FindAsync(id);
            if (turboreactor == null)
            {
                return HttpNotFound();
            }
            return View(turboreactor);
        }

        // POST: Turboreactors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Turboreactor turboreactor = await db.Turboreactors.FindAsync(id);
            db.Turboreactors.Remove(turboreactor);
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
