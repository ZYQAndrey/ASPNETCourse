namespace ASPNETCourse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "Quiz_Id", "dbo.Quizs");
            DropIndex("dbo.Questions", new[] { "Quiz_Id" });
            CreateTable(
                "dbo.QuizQuestions",
                c => new
                    {
                        Quiz_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Quiz_Id, t.Question_Id })
                .ForeignKey("dbo.Quizs", t => t.Quiz_Id, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.Quiz_Id)
                .Index(t => t.Question_Id);
            
            DropColumn("dbo.Questions", "Quiz_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "Quiz_Id", c => c.Int());
            DropForeignKey("dbo.QuizQuestions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.QuizQuestions", "Quiz_Id", "dbo.Quizs");
            DropIndex("dbo.QuizQuestions", new[] { "Question_Id" });
            DropIndex("dbo.QuizQuestions", new[] { "Quiz_Id" });
            DropTable("dbo.QuizQuestions");
            CreateIndex("dbo.Questions", "Quiz_Id");
            AddForeignKey("dbo.Questions", "Quiz_Id", "dbo.Quizs", "Id");
        }
    }
}
