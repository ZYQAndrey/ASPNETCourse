namespace ASPNETCourse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class groupQuizRelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeachingGroupQuizs",
                c => new
                    {
                        TeachingGroup_Id = c.Int(nullable: false),
                        Quiz_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TeachingGroup_Id, t.Quiz_Id })
                .ForeignKey("dbo.TeachingGroups", t => t.TeachingGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.Quizs", t => t.Quiz_Id, cascadeDelete: true)
                .Index(t => t.TeachingGroup_Id)
                .Index(t => t.Quiz_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeachingGroupQuizs", "Quiz_Id", "dbo.Quizs");
            DropForeignKey("dbo.TeachingGroupQuizs", "TeachingGroup_Id", "dbo.TeachingGroups");
            DropIndex("dbo.TeachingGroupQuizs", new[] { "Quiz_Id" });
            DropIndex("dbo.TeachingGroupQuizs", new[] { "TeachingGroup_Id" });
            DropTable("dbo.TeachingGroupQuizs");
        }
    }
}
