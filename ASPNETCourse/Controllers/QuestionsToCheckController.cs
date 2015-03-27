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
    public class QuestionsToCheckController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: QuestionsToCheck
        public async Task<ActionResult> Index()
        {
            return View(await _db.QuestionsToCheck.ToListAsync());
        }

        // GET: QuestionsToCheck/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionToCheck questionToCheck = await _db.QuestionsToCheck.FindAsync(id);
            if (questionToCheck == null)
            {
                return HttpNotFound();
            }
            return View(questionToCheck);
        }

        // GET: QuestionsToCheck/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuestionsToCheck/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,Answer,Weight")] QuestionToCheckCreateView questionToCheck)
        {
            if (ModelState.IsValid)
            {
                string tempHtml = HttpUtility.HtmlDecode(questionToCheck.Description);
                questionToCheck.Description = tempHtml;

                string tempHtml1 = HttpUtility.HtmlDecode(questionToCheck.Answer);
                questionToCheck.Answer = tempHtml1;

                var newQuestion = new QuestionToCheck()
                {
                    Answer = questionToCheck.Answer,
                    Description = questionToCheck.Description,
                    Weight = questionToCheck.Weight
                };
                _db.QuestionsToCheck.Add(newQuestion);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(questionToCheck);
        }

        // GET: QuestionsToCheck/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionToCheck questionToCheck = await _db.QuestionsToCheck.FindAsync(id);
            if (questionToCheck == null)
            {
                return HttpNotFound();
            }
            return View(questionToCheck);
        }

        // POST: QuestionsToCheck/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,Answer,Weight")] QuestionToCheck questionToCheck)
        {
            if (ModelState.IsValid)
            {
                string tempHtml = HttpUtility.HtmlDecode(questionToCheck.Description);
                questionToCheck.Description = tempHtml;

                string tempHtml1 = HttpUtility.HtmlDecode(questionToCheck.Answer);
                questionToCheck.Answer = tempHtml1;

                _db.Entry(questionToCheck).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(questionToCheck);
        }

        // GET: QuestionsToCheck/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionToCheck questionToCheck = await _db.QuestionsToCheck.FindAsync(id);
            if (questionToCheck == null)
            {
                return HttpNotFound();
            }
            return View(questionToCheck);
        }

        // POST: QuestionsToCheck/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            QuestionToCheck questionToCheck = await _db.QuestionsToCheck.FindAsync(id);
            _db.QuestionsToCheck.Remove(questionToCheck);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
