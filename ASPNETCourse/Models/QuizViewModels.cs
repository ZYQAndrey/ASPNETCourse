using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETCourse.Models
{
    public class GroupCreationView
    {
        [Required]
        [Display(Name = "Group name")]
        public string Name { get; set; }
    }

    public class QuizCreateView
    {
        [Required]
        [Display(Name = "Quiz name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Begin date and time")]
        public DateTime BeginDateTime { get; set; }

        [Required]
        [Display(Name = "Finish date and time")]
        public DateTime FinishDateTime { get; set; }

        [Required]
        [Display(Name = "Quiz length in minutes")]
        public int Length { get; set; }
    }

    public class QuizEditView
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Quiz name")]
        public string Name { get; set; }

        public List<Question> QuestionsList { get; set; }

        [Required]
        [Display(Name = "Begin date and time")]
        public DateTime BeginDateTime { get; set; }

        [Required]
        [Display(Name = "Finish date and time")]
        public DateTime FinishDateTime { get; set; }

        [Required]
        [Display(Name = "Quiz length in minutes")]
        public int Length { get; set; }
    }

    public class QuestionCreateView
    {
        [Required]
        [Display(Name = "Question description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Answer option(s)")]
        public string Questions { get; set; }

        [Required]
        [Display(Name = "Right answer(s)")]
        public string Answers { get; set; }

        [Display(Name = "Question type")]
        public QuestionType Type { get; set; }
    }

    public class QuestionEditView
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Question description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Answer option(s)")]
        public string Questions { get; set; }

        [Required]
        [Display(Name = "Right answer(s)")]
        public string Answers { get; set; }

        [Display(Name = "Question type")]
        public QuestionType Type { get; set; }
    }

    public class QuestionToCheckCreateView
    {
        [Required]
        [Display(Name = "Question description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Right answer")]
        public string Answer { get; set; }

        [Display(Name = "Question weight")]
        public int Weight { get; set; }
    }

    //public class QuestionToCheckEditView
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    [Required]
    //    [Display(Name = "Question description")]
    //    public string Description { get; set; }

    //    [Required]
    //    [Display(Name = "Right answer")]
    //    public string Answer { get; set; }

    //    [Display(Name = "Question weight")]
    //    public int Weight { get; set; }
    //}

    public class AddQuestionsView
    {
        [Key]
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public List<QuestionToAdd> QuestionsList { get; set; }
    }

    public class QuestionToAdd
    {
        [Key]
        public int QuestionId { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
    }

    public class AddGroupsView
    {
        [Key]
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public List<GroupToAdd> GroupsList { get; set; }
    }

    public class GroupToAdd
    {
        [Key]
        public int GroupId { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public class AnsweredQuestion
    {
        public int QuizId { get; set; }

        public int  QuestionId { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Question")]
        public string Description { get; set; }

        [Display(Name = "Answers")]
        public List<AnswerToAnswer> Answers { get; set; }

        [Display(Name = "One of two answer")]
        public List<RadioAnswer> RadioAnswers { get; set; }

        [Display(Name = "Exact value, please")]
        public string ValueAnswer { get; set; }

        public QuestionType TheType { get; set; }

        public string QuizName { get; set; }

        public string StartTime { get; set; }

        public int TimeLength { get; set; }
    }

    public class AnswerToAnswer
    {
        public int AnswerId { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public class RadioAnswer
    {
        public string GroupName { get; set; }
        public string Value { get; set; }
        public int Id { get; set; }
        public bool Selected { get; set; }
    }

    public class AnsweredToCheckQuestion
    {
        public int QuizId { get; set; }

        public int QuestionId { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Question")]
        public string Description { get; set; }

        [Display(Name = "Exact value, please")]
        public string Answer { get; set; }
        
        public string QuizName { get; set; }

        public string StartTime { get; set; }

        public int TimeLength { get; set; }
    }
}
