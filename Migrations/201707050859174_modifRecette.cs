namespace Cuillere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifRecette : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RecetteCategories", "Recette_RecetteId", "dbo.Recettes");
            DropForeignKey("dbo.RecetteCategories", "Category_CategoryId", "dbo.Categories");
            DropIndex("dbo.RecetteCategories", new[] { "Recette_RecetteId" });
            DropIndex("dbo.RecetteCategories", new[] { "Category_CategoryId" });
            CreateIndex("dbo.Recettes", "CategoryId");
            AddForeignKey("dbo.Recettes", "CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
            DropTable("dbo.RecetteCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RecetteCategories",
                c => new
                    {
                        Recette_RecetteId = c.Int(nullable: false),
                        Category_CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recette_RecetteId, t.Category_CategoryId });
            
            DropForeignKey("dbo.Recettes", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Recettes", new[] { "CategoryId" });
            CreateIndex("dbo.RecetteCategories", "Category_CategoryId");
            CreateIndex("dbo.RecetteCategories", "Recette_RecetteId");
            AddForeignKey("dbo.RecetteCategories", "Category_CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
            AddForeignKey("dbo.RecetteCategories", "Recette_RecetteId", "dbo.Recettes", "RecetteId", cascadeDelete: true);
        }
    }
}
