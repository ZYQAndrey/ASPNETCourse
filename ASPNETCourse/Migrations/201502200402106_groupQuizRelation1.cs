namespace ASPNETCourse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class groupQuizRelation1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.TeachingGroups");
            DropIndex("dbo.AspNetUsers", new[] { "Group_Id" });
            AlterColumn("dbo.AspNetUsers", "Group_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Group_Id");
            AddForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.TeachingGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.TeachingGroups");
            DropIndex("dbo.AspNetUsers", new[] { "Group_Id" });
            AlterColumn("dbo.AspNetUsers", "Group_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "Group_Id");
            AddForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.TeachingGroups", "Id", cascadeDelete: true);
        }
    }
}
