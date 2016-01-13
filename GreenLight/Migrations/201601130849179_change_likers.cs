namespace GreenLight.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_likers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "LikedCommentId", "dbo.Comments");
            DropIndex("dbo.AspNetUsers", new[] { "LikedCommentId" });
            AddColumn("dbo.Comments", "Likers", c => c.String());
            DropColumn("dbo.AspNetUsers", "LikedCommentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "LikedCommentId", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "Likers");
            CreateIndex("dbo.AspNetUsers", "LikedCommentId");
            AddForeignKey("dbo.AspNetUsers", "LikedCommentId", "dbo.Comments", "Id", cascadeDelete: true);
        }
    }
}
