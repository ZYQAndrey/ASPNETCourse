using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;
using ASPNETCourse.Models;
using Microsoft.AspNet.Identity;

namespace ASPNETCourse.Controllers
{
    public class ExamController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/Exam/1
        [ResponseType(typeof(AnsweredQuestion))]
        public async Task<IHttpActionResult> Get(int? id)
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

                var nextQuestion = new AnsweredQuestion()
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

                if (nextQuestion.TheType == QuestionType.Multy)
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

                    nextQuestion.Answers = answersList;
                }

                if (nextQuestion.TheType == QuestionType.Radio)
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

                    nextQuestion.RadioAnswers = answersList;
                }

                if (nextQuestion.TheType == QuestionType.Value) nextQuestion.ValueAnswer = "";

                return this.Ok(nextQuestion);
            }
            else
            {
                return this.NotFound();
            }
        }

        // POST: api/Exam
        [ResponseType(typeof(AnsweredQuestion))]
        public async Task<IHttpActionResult> Post(AnsweredQuestion model)
        {
            var user = _db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
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

                //if (model.RadioAnswers != null)
                //{
                //    newAnswer.Answers += Request.Form["Radio"] + ";";
                //}

                if (model.ValueAnswer != null)
                {
                    newAnswer.Answers = model.ValueAnswer;
                }

                newAnswer.Correctness = newAnswer.Answers == _db.Questions.Find(model.QuestionId).Answers;

                _db.Answers.Add(newAnswer);
                await _db.SaveChangesAsync();

                return this.Ok<bool>(newAnswer.Correctness);
            }
            else
            {
                return this.BadRequest(this.ModelState);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._db.Dispose();
            }

            base.Dispose(disposing);
        }

    }
}
