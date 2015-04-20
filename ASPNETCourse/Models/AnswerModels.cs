using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETCourse.Models
{
    public class IdAndTime
    {
        public int? Id { get; set; }
        public string StartTime { get; set; }
        public string QuizName { get; set; }
        public int QuestionsNumber { get; set; }
    }

    public class StudentsResultsViewModel
    {
        public int Id { get; set; }
        [Display(Name="Student name")]
        public string FullName { get; set; }

        [Display(Name="Group")]
        public string GroupName { get; set; }

        [Display(Name="Score")]
        public int Score { get; set; }

        public DateTime ExamDate { get; set; }
    }
}
