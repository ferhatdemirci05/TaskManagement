namespace DataAccessLayer.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class TaskManager : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ContactName = c.String(nullable: false, maxLength: 50, unicode: false),
                        CustomerEmail = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 15, unicode: false),
                        LastName = c.String(nullable: false, maxLength: 15, unicode: false),
                        PositionID = c.Int(nullable: false),
                        Password = c.String(nullable: false, maxLength: 15, unicode: false),
                        EMail = c.String(nullable: false, maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Positions", t => t.PositionID, cascadeDelete: true)
                .Index(t => t.PositionID);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PositionName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ProjectDetail = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CustomerID = c.Int(nullable: false),
                        ManagerID = c.Int(nullable: false),
                        PlannedStartDate = c.DateTime(nullable: false),
                        PlannedEndDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.ManagerID, cascadeDelete: true)
                .Index(t => t.CustomerID)
                .Index(t => t.ManagerID);
            
            CreateTable(
                "dbo.ProjectEmployees",
                c => new
                    {
                        ProjectID = c.Int(nullable: false),
                        EmployeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectID, t.EmployeeID })
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID)
                .Index(t => t.ProjectID)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RequestDetail = c.String(nullable: false, maxLength: 8000, unicode: false),
                        ProjectID = c.Int(nullable: false),
                        EmployeeID = c.Int(nullable: false),
                        RequestTypeID = c.Int(nullable: false),
                        State = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "True")
                                },
                            }),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID)
                .ForeignKey("dbo.RequestTypes", t => t.RequestTypeID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.EmployeeID)
                .Index(t => t.RequestTypeID);
            
            CreateTable(
                "dbo.RequestTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Works",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WorkDetail = c.String(nullable: false, maxLength: 8000, unicode: false),
                        ProjectID = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false, storeType: "date"),
                        StartDate = c.DateTime(storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        ManagerID = c.Int(nullable: false),
                        EmployeeID = c.Int(),
                        WorkStatusID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.Employees", t => t.ManagerID)
                .ForeignKey("dbo.Projects", t => t.ProjectID)
                .ForeignKey("dbo.WorkStatus", t => t.WorkStatusID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.ManagerID)
                .Index(t => t.EmployeeID)
                .Index(t => t.WorkStatusID);
            
            CreateTable(
                "dbo.WorkStatus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StatusName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Works", "WorkStatusID", "dbo.WorkStatus");
            DropForeignKey("dbo.Works", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Works", "ManagerID", "dbo.Employees");
            DropForeignKey("dbo.Works", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Requests", "RequestTypeID", "dbo.RequestTypes");
            DropForeignKey("dbo.Requests", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Requests", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.ProjectEmployees", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.ProjectEmployees", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Projects", "ManagerID", "dbo.Employees");
            DropForeignKey("dbo.Projects", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Employees", "PositionID", "dbo.Positions");
            DropIndex("dbo.Works", new[] { "WorkStatusID" });
            DropIndex("dbo.Works", new[] { "EmployeeID" });
            DropIndex("dbo.Works", new[] { "ManagerID" });
            DropIndex("dbo.Works", new[] { "ProjectID" });
            DropIndex("dbo.Requests", new[] { "RequestTypeID" });
            DropIndex("dbo.Requests", new[] { "EmployeeID" });
            DropIndex("dbo.Requests", new[] { "ProjectID" });
            DropIndex("dbo.ProjectEmployees", new[] { "EmployeeID" });
            DropIndex("dbo.ProjectEmployees", new[] { "ProjectID" });
            DropIndex("dbo.Projects", new[] { "ManagerID" });
            DropIndex("dbo.Projects", new[] { "CustomerID" });
            DropIndex("dbo.Employees", new[] { "PositionID" });
            DropTable("dbo.WorkStatus");
            DropTable("dbo.Works");
            DropTable("dbo.RequestTypes");
            DropTable("dbo.Requests",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "State",
                        new Dictionary<string, object>
                        {
                            { "DefaultValue", "True" },
                        }
                    },
                });
            DropTable("dbo.ProjectEmployees");
            DropTable("dbo.Projects");
            DropTable("dbo.Positions");
            DropTable("dbo.Employees");
            DropTable("dbo.Customers");
        }
    }
}
