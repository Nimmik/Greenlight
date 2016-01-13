namespace GreenLight.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Likers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LikedCommentId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "LikedCommentId");
            AddForeignKey("dbo.AspNetUsers", "LikedCommentId", "dbo.Comments", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "LikedCommentId", "dbo.Comments");
            DropIndex("dbo.AspNetUsers", new[] { "LikedCommentId" });
            DropColumn("dbo.AspNetUsers", "LikedCommentId");
        }
    }
}
