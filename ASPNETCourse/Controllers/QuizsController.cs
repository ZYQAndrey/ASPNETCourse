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
    public class QuizsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Quizs
        public async Task<ActionResult> Index()
        {
            return View(await _db.Quizs.ToListAsync());
        }

        // GET: Quizs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var quiz = await _db.Quizs.Include("QuestionsList").Include("QuestionsToCheckList").Include("GroupsList").FirstOrDefaultAsync(m => m.Id == id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // GET: Quizs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quizs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,BeginDateTime,FinishDateTime,Length")] QuizCreateView quiz)
        {
            if (ModelState.IsValid)
            {
                var newQuiz = new Quiz()
                {
                    BeginDateTime = quiz.BeginDateTime,
                    FinishDateTime = quiz.FinishDateTime,
                    Length = quiz.Length,
                    Name = quiz.Name
                };
                _db.Quizs.Add(newQuiz);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(quiz);
        }

        // GET: Quizs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var quiz = await _db.Quizs.FindAsync(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST: Quizs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,BeginDateTime,FinishDateTime,Length")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(quiz).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(quiz);
        }

        // GET: Quizs/AddQuestions/5
        public async Task<ActionResult> AddQuestions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var quiz = await _db.Quizs.Include("QuestionsList").FirstOrDefaultAsync(m => m.Id == id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            var questionsList = await _db.Questions.ToListAsync();
            var model = new AddQuestionsView
            {
                QuizId = quiz.Id,
                QuizName = quiz.Name,
                QuestionsList = questionsList.Select(question => new QuestionToAdd()
                {
                    QuestionId = question.Id,
                    Description = question.Description,
                    Selected = false
                }).ToList()
            };
            foreach (var questionToAdd in model.QuestionsList.Where(questionToAdd => quiz.QuestionsList.Contains(_db.Questions.Find(questionToAdd.QuestionId))))
            {
                questionToAdd.Selected = true;
            }
            ViewBag.Name = quiz.Name;
            ViewBag.QuizId = quiz.Id;
            return View(model);
        }

        // POST: Quizs/AddQuestions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddQuestions(AddQuestionsView model)
        {
            if (ModelState.IsValid)
            {
                var quiz = _db.Quizs.Include("QuestionsList").FirstOrDefault(m => m.Id == model.QuizId);
                if (quiz == null) return View(model);
                if (quiz.QuestionsList == null) quiz.QuestionsList = new List<Question>();
                else
                {
                    quiz.QuestionsList.Clear();
                    _db.SaveChanges();
                }

                foreach (var questionToAdd in model.QuestionsList.Where(questionToAdd => questionToAdd.Selected))
                {
                    quiz.QuestionsList.Add(_db.Questions.Find(questionToAdd.QuestionId));
                }
                
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Quizs/AddQuestions/5
        public async Task<ActionResult> AddQuestionsToCheck(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var quiz = await _db.Quizs.Include("QuestionsToCheckList").FirstOrDefaultAsync(m => m.Id == id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            var questionsList = await _db.QuestionsToCheck.ToListAsync();
            var model = new AddQuestionsView
            {
                QuizId = quiz.Id,
                QuizName = quiz.Name,
                QuestionsList = questionsList.Select(question => new QuestionToAdd()
                {
                    QuestionId = question.Id,
                    Description = question.Description,
                    Selected = false
                }).ToList()
            };
            foreach (var questionToAdd in model.QuestionsList.Where(questionToAdd => quiz.QuestionsToCheckList.Contains(_db.QuestionsToCheck.Find(questionToAdd.QuestionId))))
            {
                questionToAdd.Selected = true;
            }
            ViewBag.Name = quiz.Name;
            ViewBag.QuizId = quiz.Id;
            return View(model);
        }

        // POST: Quizs/AddQuestions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddQuestionsToCheck(AddQuestionsView model)
        {
            if (ModelState.IsValid)
            {
                var quiz = _db.Quizs.Include("QuestionsToCheckList").FirstOrDefault(m => m.Id == model.QuizId);
                if (quiz == null) return View(model);
                if (quiz.QuestionsToCheckList == null) quiz.QuestionsToCheckList = new List<QuestionToCheck>();
                else
                {
                    quiz.QuestionsToCheckList.Clear();
                    _db.SaveChanges();
                }

                foreach (var questionToAdd in model.QuestionsList.Where(questionToAdd => questionToAdd.Selected))
                {
                    quiz.QuestionsToCheckList.Add(_db.QuestionsToCheck.Find(questionToAdd.QuestionId));
                }

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<ActionResult> AssignToGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var quiz = await _db.Quizs.Include("GroupsList").FirstOrDefaultAsync(m => m.Id == id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            var qroupsList = await _db.Groups.ToListAsync();
            var model = new AddGroupsView
            {
                QuizId = quiz.Id,
                QuizName = quiz.Name,
                GroupsList = qroupsList.Select(group => new GroupToAdd()
                {
                    GroupId = group.Id,
                    Name = group.Name,
                    Selected = false
                }).ToList()
            };
            foreach (var group in model.GroupsList.Where(group => quiz.GroupsList.Contains(_db.Groups.Find(group.GroupId))))
            {
                group.Selected = true;
            }
            ViewBag.Name = quiz.Name;
            ViewBag.QuizId = quiz.Id;
            return View(model);
        }

        // POST: Quizs/AssignToGroup/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignToGroup(AddGroupsView model)
        {
            if (ModelState.IsValid)
            {
                var quiz = _db.Quizs.Include("GroupsList").FirstOrDefault(m => m.Id == model.QuizId);
                if (quiz == null) return View(model);
                if (quiz.GroupsList == null) quiz.GroupsList = new List<TeachingGroup>();
                else
                {
                    quiz.GroupsList.Clear();
                    _db.SaveChanges();
                }

                foreach (var groupToAdd in model.GroupsList.Where(groupToAdd => groupToAdd.Selected))
                {
                    quiz.GroupsList.Add(_db.Groups.Find(groupToAdd.GroupId));
                }

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Quizs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var quiz = await _db.Quizs.FindAsync(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST: Quizs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var quiz = await _db.Quizs.FindAsync(id);
            _db.Quizs.Remove(quiz);
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
