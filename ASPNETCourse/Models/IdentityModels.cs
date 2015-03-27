using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASPNETCourse.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //[Required]
        public int GroupId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public List<Answer> Answers { get; set; }

        public List<AnswerToCheck> AnswersToCheck { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Quiz> Quizs { get; set; }
        
        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionToCheck> QuestionsToCheck { get; set; }

        public DbSet<TeachingGroup> Groups { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<AnswerToCheck> AnswersToCheck { get; set; }
        
    }
}