namespace GreenLight.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addviewmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCommentLikes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Up = c.Boolean(nullable: false),
                        LikeUserId = c.String(maxLength: 128),
                        LikedCommentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.LikeUserId)
                .ForeignKey("dbo.Comments", t => t.LikedCommentId, cascadeDelete: true)
                .Index(t => t.LikeUserId)
                .Index(t => t.LikedCommentId);
            
            DropColumn("dbo.Comments", "Likes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "Likes", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserCommentLikes", "LikedCommentId", "dbo.Comments");
            DropForeignKey("dbo.UserCommentLikes", "LikeUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserCommentLikes", new[] { "LikedCommentId" });
            DropIndex("dbo.UserCommentLikes", new[] { "LikeUserId" });
            DropTable("dbo.UserCommentLikes");
        }
    }
}
