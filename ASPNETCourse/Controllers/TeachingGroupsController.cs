using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPNETCourse.Models;

namespace ASPNETCourse.Controllers
{
    [Authorize(Users = "a.v.andreev@hotmail.com")]
    public class TeachingGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TeachingGroups
        public async Task<ActionResult> Index()
        {
            return View(await db.Groups.ToListAsync());
        }

        // GET: TeachingGroups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachingGroup teachingGroup = await db.Groups.FindAsync(id);
            if (teachingGroup == null)
            {
                return HttpNotFound();
            }
            return View(teachingGroup);
        }

        // GET: TeachingGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeachingGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] TeachingGroup teachingGroup)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(teachingGroup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(teachingGroup);
        }

        // GET: TeachingGroups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachingGroup teachingGroup = await db.Groups.FindAsync(id);
            if (teachingGroup == null)
            {
                return HttpNotFound();
            }
            return View(teachingGroup);
        }

        // POST: TeachingGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] TeachingGroup teachingGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teachingGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(teachingGroup);
        }

        // GET: TeachingGroups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachingGroup teachingGroup = await db.Groups.FindAsync(id);
            if (teachingGroup == null)
            {
                return HttpNotFound();
            }
            return View(teachingGroup);
        }

        // POST: TeachingGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TeachingGroup teachingGroup = await db.Groups.FindAsync(id);
            db.Groups.Remove(teachingGroup);
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
