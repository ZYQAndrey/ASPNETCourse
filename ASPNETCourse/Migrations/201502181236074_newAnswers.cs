namespace ASPNETCourse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newAnswers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionToChecks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Answer = c.String(nullable: false),
                        Weight = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AnswerToChecks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Answer = c.String(nullable: false),
                        AnswerTime = c.DateTime(nullable: false),
                        Correctness = c.Boolean(nullable: false),
                        Question_Id = c.Int(nullable: false),
                        Quiz_Id = c.Int(nullable: false),
                        Student_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionToChecks", t => t.Question_Id, cascadeDelete: true)
                .ForeignKey("dbo.Quizs", t => t.Quiz_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id, cascadeDelete: true)
                .Index(t => t.Question_Id)
                .Index(t => t.Quiz_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.QuestionToCheckQuizs",
                c => new
                    {
                        QuestionToCheck_Id = c.Int(nullable: false),
                        Quiz_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuestionToCheck_Id, t.Quiz_Id })
                .ForeignKey("dbo.QuestionToChecks", t => t.QuestionToCheck_Id, cascadeDelete: true)
                .ForeignKey("dbo.Quizs", t => t.Quiz_Id, cascadeDelete: true)
                .Index(t => t.QuestionToCheck_Id)
                .Index(t => t.Quiz_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnswerToChecks", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AnswerToChecks", "Quiz_Id", "dbo.Quizs");
            DropForeignKey("dbo.AnswerToChecks", "Question_Id", "dbo.QuestionToChecks");
            DropForeignKey("dbo.QuestionToCheckQuizs", "Quiz_Id", "dbo.Quizs");
            DropForeignKey("dbo.QuestionToCheckQuizs", "QuestionToCheck_Id", "dbo.QuestionToChecks");
            DropIndex("dbo.QuestionToCheckQuizs", new[] { "Quiz_Id" });
            DropIndex("dbo.QuestionToCheckQuizs", new[] { "QuestionToCheck_Id" });
            DropIndex("dbo.AnswerToChecks", new[] { "Student_Id" });
            DropIndex("dbo.AnswerToChecks", new[] { "Quiz_Id" });
            DropIndex("dbo.AnswerToChecks", new[] { "Question_Id" });
            DropTable("dbo.QuestionToCheckQuizs");
            DropTable("dbo.AnswerToChecks");
            DropTable("dbo.QuestionToChecks");
        }
    }
}
