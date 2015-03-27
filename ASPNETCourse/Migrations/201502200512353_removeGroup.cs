namespace ASPNETCourse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeGroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.TeachingGroups");
            DropIndex("dbo.AspNetUsers", new[] { "Group_Id" });
            AddColumn("dbo.AspNetUsers", "GroupId", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "Group_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Group_Id", c => c.Int());
            DropColumn("dbo.AspNetUsers", "GroupId");
            CreateIndex("dbo.AspNetUsers", "Group_Id");
            AddForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.TeachingGroups", "Id");
        }
    }
}
