using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPNETCourse.Migrations;
using ASPNETCourse.Models;
using Microsoft.AspNet.Identity;

namespace ASPNETCourse.Controllers
{
    [Authorize]
    public class AnswersController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Answers
        public async Task<ActionResult> ExamIndex()
        {
            var user = _db.Users.Find(User.Identity.GetUserId());
            var group = _db.Groups.Include("QuizesList").First(d => d.Id == user.GroupId);
            var quizesActing = await _db.Quizs.Where(d => d.BeginDateTime <= DateTime.Now && d.FinishDateTime >= DateTime.Now).ToListAsync();
            var myActingQuizes = quizesActing.Where(quiz => (@group.QuizesList != null) && (@group.QuizesList.Contains(quiz))).ToList();
            
            var quizesPast = await _db.Quizs.Where(d => d.FinishDateTime < DateTime.Now).ToListAsync();
            var myPastQuizes = quizesPast.Where(quiz => (@group.QuizesList != null) && (@group.QuizesList.Contains(quiz))).ToList();
            
            var quizesFuture = await _db.Quizs.Where(d => d.BeginDateTime > DateTime.Now).ToListAsync();
            var myFutureQuizes = quizesFuture.Where(quiz => (@group.QuizesList != null) && (@group.QuizesList.Contains(quiz))).ToList();
            
            ViewBag.actingExams = myActingQuizes;
            ViewBag.pastExams = myPastQuizes;
            ViewBag.futureExams = myFutureQuizes;
            return View();
        }

        // GET: Answers
        public async Task<ActionResult> ActiveExam(int? id)
        {
            var user = _db.Users.Find(User.Identity.GetUserId());
            var myQuiz = await _db.Quizs.Include("QuestionsList").FirstOrDefaultAsync(d => d.Id == id);
            var myAnswers = await _db.Answers.Where(d => d.Student.UserName == user.UserName && d.Quiz.Id == id).ToListAsync();
            var myQuestions = myAnswers.Select(answer => answer.Question).ToList();

            if (myQuiz.QuestionsList.Count > myAnswers.Count)
            {
                var notFound = true;
                var temp = 0;
                while (notFound)
                {
                    var random = new Random();
                    temp = random.Next(myQuiz.QuestionsList.Count);
                    if (myQuestions.All(d => d.Id != myQuiz.QuestionsList[temp].Id)) notFound = false;
                }

                var model = new AnsweredQuestion()
                {
                    Description = myQuiz.QuestionsList[temp].Description,
                    QuestionId = myQuiz.QuestionsList[temp].Id,
                    QuizId = myQuiz.Id,
                    UserName = user.UserName,
                    TheType = myQuiz.QuestionsList[temp].Type,
                    QuizName = myQuiz.Name,
                    //StartTime = (myQuiz.Length * 1000000 * 60).ToString(),
                    StartTime = DateTime.Now.AddMinutes(myQuiz.Length).Year + "/" + DateTime.Now.AddMinutes(myQuiz.Length).Month + "/" + DateTime.Now.AddMinutes(myQuiz.Length).Day + " " + (DateTime.Now.AddMinutes(myQuiz.Length).Hour + ":" + DateTime.Now.AddMinutes(myQuiz.Length).Minute),
                    TimeLength = myQuiz.Length
                };

                if (model.TheType == QuestionType.Multy)
                {
                    var answersList = new List<AnswerToAnswer>();
                    var answersString = myQuiz.QuestionsList[temp].Questions;
                    int counter = 0;
                    while (!String.IsNullOrEmpty(answersString))
                    {
                        answersString = answersString.Trim();
                        answersList.Add(new AnswerToAnswer()
                        {
                            AnswerId = counter,
                            Name = answersString.Substring(0, answersString.IndexOf(';')),
                            Selected = false
                        });
                        counter++;
                        answersString = answersString.IndexOf(';') < answersString.Length + 1
                            ? answersString.Substring(answersString.IndexOf(';') + 1,
                                answersString.Length - answersString.IndexOf(';') - 1)
                            : "";
                    }

                    model.Answers = answersList;
                }

                if (model.TheType == QuestionType.Radio)
                {
                    var answersList = new List<RadioAnswer>();
                    var answersString = myQuiz.QuestionsList[temp].Questions;
                    int counter = 0;
                    while (!String.IsNullOrEmpty(answersString))
                    {
                        answersString = answersString.Trim();
                        answersList.Add(new RadioAnswer()
                        {
                            Value = answersString.Substring(0, answersString.IndexOf(';')),
                            GroupName = "questionGroup",
                            Id = counter,
                            Selected = false
                        });
                        counter++;
                        answersString = answersString.IndexOf(';') < answersString.Length + 1
                            ? answersString.Substring(answersString.IndexOf(';') + 1,
                                answersString.Length - answersString.IndexOf(';') - 1)
                            : "";
                    }

                    model.RadioAnswers = answersList;
                }

                if (model.TheType == QuestionType.Value) model.ValueAnswer = "";

                return View(model);
            }
            else
            {
                return RedirectToAction("ActiveExamToCheck", new { id = id, startTime = DateTime.Now.AddMinutes(myQuiz.Length).Year + "/" + DateTime.Now.AddMinutes(myQuiz.Length).Month + "/" + DateTime.Now.AddMinutes(myQuiz.Length).Day + " " + (DateTime.Now.AddMinutes(myQuiz.Length).Hour + ":" + DateTime.Now.AddMinutes(myQuiz.Length).Minute) });
            }
        }

        // POST: Answers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ActiveExam(AnsweredQuestion model, FormCollection formCollection)
        {
            var user = _db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                //var questions = _db.Answers.Include("Question").Where(d => d.Student.UserName == user.UserName && d.Quiz.Id == model.QuizId).Select(answer => answer.Question).ToList();
                //if (!questions.Any(d => d.Id == model.QuestionId))
                //{
                    var newAnswer = new Answer()
                    {
                        AnswerTime = DateTime.Now,
                        Question = _db.Questions.Find(model.QuestionId),
                        Quiz = _db.Quizs.Find(model.QuizId),
                        Student = _db.Users.Find(User.Identity.GetUserId()),
                    };

                    if (model.Answers != null)
                    {
                        foreach (var answer in model.Answers.Where(answer => answer.Selected))
                        {
                            newAnswer.Answers += answer.Name + ";";
                        }
                    }

                    if (model.RadioAnswers != null)
                    {
                        newAnswer.Answers += Request.Form["Radio"] + ";";
                    }

                    if (model.ValueAnswer != null)
                    {
                        newAnswer.Answers = model.ValueAnswer;
                    }

                    newAnswer.Correctness = newAnswer.Answers == _db.Questions.Find(model.QuestionId).Answers;

                    _db.Answers.Add(newAnswer);
                    _db.SaveChanges();
                //}
            }
            

            var myQuiz = await _db.Quizs.Include("QuestionsList").FirstOrDefaultAsync(d => d.Id == model.QuizId);
            var myAnswers = await _db.Answers.Where(d => d.Student.UserName == user.UserName && d.Quiz.Id == model.QuizId).ToListAsync();
            var myQuestions = myAnswers.Select(answer => answer.Question).ToList();

            if (myQuiz.QuestionsList.Count > myAnswers.Count)
            {
                var notFound = true;
                var temp = 0;
                while (notFound)
                {
                    var random = new Random();
                    temp = random.Next(myQuiz.QuestionsList.Count);
                    if (myQuestions.All(d => d.Id != myQuiz.QuestionsList[temp].Id)) notFound = false;
                }
                var question = myQuiz.QuestionsList[temp];
                model.Description = question.Description;
                model.QuestionId = question.Id;
                model.QuizId = myQuiz.Id;
                model.UserName = user.UserName;
                model.TheType = question.Type;
                model.QuizName = myQuiz.Name;


                if (model.TheType == QuestionType.Multy)
                {
                    var answersList = new List<AnswerToAnswer>();
                    var answersString = question.Questions;
                    int counter = 0;
                    while (!String.IsNullOrEmpty(answersString))
                    {
                        answersString = answersString.Trim();
                        answersList.Add(new AnswerToAnswer()
                        {
                            AnswerId = counter,
                            Name = answersString.Substring(0, answersString.IndexOf(';')),
                            Selected = false
                        });
                        counter++;
                        answersString = answersString.IndexOf(';') < answersString.Length + 1
                            ? answersString.Substring(answersString.IndexOf(';') + 1,
                                answersString.Length - answersString.IndexOf(';') - 1)
                            : "";
                    }

                    model.Answers = answersList;
                }

                if (model.TheType == QuestionType.Radio)
                {
                    var answersList = new List<RadioAnswer>();
                    var answersString = question.Questions;
                    int counter = 0;
                    while (!String.IsNullOrEmpty(answersString))
                    {
                        answersString = answersString.Trim();
                        answersList.Add(new RadioAnswer()
                        {
                            Value = answersString.Substring(0, answersString.IndexOf(';')),
                            GroupName = "questionGroup",
                            Id = counter,
                            Selected = false
                        });
                        counter++;
                        answersString = answersString.IndexOf(';') < answersString.Length + 1
                            ? answersString.Substring(answersString.IndexOf(';') + 1,
                                answersString.Length - answersString.IndexOf(';') - 1)
                            : "";
                    }

                    model.RadioAnswers = answersList;
                }

                if (model.TheType == QuestionType.Value) model.ValueAnswer = "";

                return View(model);
            }
            else return RedirectToAction("ActiveExamToCheck", new { id = model.QuizId, startTime = model.StartTime});
        }

        public async Task<ActionResult> ActiveExamToCheck(int? id, string startTime)
        {
            var user = _db.Users.Find(User.Identity.GetUserId());
            var myQuiz = await _db.Quizs.Include("QuestionsToCheckList").FirstOrDefaultAsync(d => d.Id == id);
            var myAnswers = await _db.AnswersToCheck.Where(d => d.Student.UserName == user.UserName && d.Quiz.Id == id).ToListAsync();
            var myQuestions = myAnswers.Select(answer => answer.Question).ToList();

            if (myQuiz.QuestionsToCheckList.Count > myAnswers.Count)
            {
                var notFound = true;
                var temp = 0;
                while (notFound)
                {
                    var random = new Random();
                    temp = random.Next(myQuiz.QuestionsToCheckList.Count);
                    if (myQuestions.All(d => d.Id != myQuiz.QuestionsToCheckList[temp].Id)) notFound = false;
                }

                var model = new AnsweredToCheckQuestion()
                {
                    Description = myQuiz.QuestionsToCheckList[temp].Description,
                    QuestionId = myQuiz.QuestionsToCheckList[temp].Id,
                    QuizId = myQuiz.Id,
                    UserName = user.UserName,
                    QuizName = myQuiz.Name,
                    Answer = "",
                    StartTime = startTime,
                    TimeLength = myQuiz.Length
                };

                return View(model);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ActiveExamToCheck(AnsweredToCheckQuestion model)
        {
            return View();
        }

        // GET: Answers
        public async Task<ActionResult> PastExam(int? id)
        {
            var answers = await _db.Answers.Where(d => d.Quiz.Id == id && d.Student.Email == User.Identity.Name).ToListAsync();
            var answersToCheck = await _db.AnswersToCheck.Where(d => d.Quiz.Id == id && d.Student.Email == User.Identity.Name).ToListAsync();

            ViewBag.Answers = answers;
            ViewBag.AnswersToCheck = answersToCheck;
            ViewBag.QuizName = _db.Quizs.Find(id).Name;

            var resultToCheck = answersToCheck.Where(answer => answer.Correctness == true).Sum(answer => answer.Question.Weight);
            ViewBag.Result = answers.Count(d => d.Correctness == true) + resultToCheck;

            var startTime = answers.OrderBy(d => d.AnswerTime).First().AnswerTime;
            var finishTime1 = answers.OrderBy(d => d.AnswerTime).Last().AnswerTime;
            var finishTime2 = answersToCheck.OrderBy(d => d.AnswerTime).Last().AnswerTime;
            var finishTime = finishTime1 > finishTime2 ? finishTime1 : finishTime2;

            var spent = finishTime - startTime;
            ViewBag.StartTime = startTime;
            ViewBag.FinishTime = finishTime;
            ViewBag.TimeSpan = spent;

            return View();
        }

        // GET: Answers
        [Authorize(Users = "a.v.andreev@hotmail.com")]
        public async Task<ActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                //var quizes = db.Quizs.Where(d => d.)
            }
            return View(await _db.Answers.ToListAsync());
        }

        // GET: Answers/Delete/5
        [Authorize(Users = "a.v.andreev@hotmail.com")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = await _db.Answers.FindAsync(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Answer answer = await _db.Answers.FindAsync(id);
            _db.Answers.Remove(answer);
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
