using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETCourse.Models
{
    public class TeachingGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Group name")]
        public string Name { get; set; }

        public List<Quiz> QuizesList { get; set; }
    }

    public enum QuestionType
    {
        Radio,
        Multy,
        Value
    }

    public class ForDropDown
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }

    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name="Question description")]
        public string Description { get; set; }

        [Required]
        [Display(Name="Answer option(s)")]
        public string Questions { get; set; }

        [Required]
        [Display(Name="Right answer(s)")]
        public string Answers { get; set; }

        [Display(Name="Question type")]
        public QuestionType Type { get; set; }

        [Display(Name="List of quizes")]
        public List<Quiz> QuizesList { get; set; }
    }

    public class QuestionToCheck
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Question description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Right answer")]
        public string Answer { get; set; }

        [Display(Name = "Question weight")]
        public int Weight { get; set; }

        [Display(Name = "List of quizes")]
        public List<Quiz> QuizesList { get; set; }
    }

    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Quiz name")]
        public string Name { get; set; }

        [Display(Name= "List of questions")]
        public List<Question> QuestionsList { get; set; }

        [Display(Name = "List of questions to check")]
        public List<QuestionToCheck> QuestionsToCheckList { get; set; }

        [Required]
        [Display(Name = "Begin date and time")]
        public DateTime BeginDateTime { get; set; }

        [Required]
        [Display(Name = "Finish date and time")]
        public DateTime FinishDateTime { get; set; }

        [Required]
        [Display(Name = "Quiz length in minutes")]
        public int Length { get; set; }

        [Display(Name = "Groups assigned")]
        public List<TeachingGroup> GroupsList { get; set; } 
    }

    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Question Question { get; set; }

        [Required]
        public string Answers { get; set; }

        [Required]
        public Quiz Quiz { get; set; }

        [Required]
        public ApplicationUser Student { get; set; }

        public DateTime AnswerTime { get; set; }

        public bool Correctness { get; set; }
    }

    public class AnswerToCheck
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public QuestionToCheck Question { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        public Quiz Quiz { get; set; }

        [Required]
        public ApplicationUser Student { get; set; }

        public DateTime AnswerTime { get; set; }

        public bool Correctness { get; set; }
    }
}
