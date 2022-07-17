namespace TodoManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Todoes",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        value = c.String(nullable: false),
                        completed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.id, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Todoes", new[] { "id" });
            DropTable("dbo.Todoes");
        }
    }
}
