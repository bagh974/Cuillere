namespace Cuillere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajoutEnum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecetteDetails", "unite", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecetteDetails", "unite");
        }
    }
}
