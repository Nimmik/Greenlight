namespace GreenLight.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stringToList : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Comments", "Likers");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "Likers", c => c.String());
        }
    }
}
