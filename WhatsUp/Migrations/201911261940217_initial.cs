namespace WhatsUp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        MobileNumber = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AccountId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactId = c.Int(nullable: false, identity: true),
                        OwnerAccountId = c.Int(),
                        ContactOwnerId = c.Int(nullable: false),
                        ContactName = c.String(nullable: false),
                        MobileNumber = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ContactId)
                .ForeignKey("dbo.Accounts", t => t.OwnerAccountId)
                .ForeignKey("dbo.Accounts", t => t.ContactOwnerId, cascadeDelete: false)
                .Index(t => t.OwnerAccountId)
                .Index(t => t.ContactOwnerId);
            
            CreateTable(
                "dbo.GroupMessages",
                c => new
                    {
                        GroupMessageId = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        SenderId = c.Int(nullable: false),
                        TextMessage = c.String(),
                        MessageSent = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GroupMessageId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: false)
                .ForeignKey("dbo.Accounts", t => t.SenderId, cascadeDelete: false)
                .Index(t => t.GroupId)
                .Index(t => t.SenderId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        GroupOwnerAccountId = c.Int(nullable: false),
                        GroupName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId)
                .ForeignKey("dbo.Accounts", t => t.GroupOwnerAccountId, cascadeDelete: false)
                .Index(t => t.GroupOwnerAccountId);
            
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        ChatId = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChatId)
                .ForeignKey("dbo.Accounts", t => t.ReceiverId, cascadeDelete: false)
                .ForeignKey("dbo.Accounts", t => t.SenderId, cascadeDelete: false)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        ChatId = c.Int(nullable: false),
                        TextMessage = c.String(nullable: false),
                        MessageSent = c.DateTime(nullable: false),
                        SenderIsReceiver = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: false)
                .Index(t => t.ChatId);
            
            CreateTable(
                "dbo.AccountGroups",
                c => new
                    {
                        Account_AccountId = c.Int(nullable: false),
                        Group_GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Account_AccountId, t.Group_GroupId })
                .ForeignKey("dbo.Accounts", t => t.Account_AccountId, cascadeDelete: false)
                .ForeignKey("dbo.Groups", t => t.Group_GroupId, cascadeDelete: false)
                .Index(t => t.Account_AccountId)
                .Index(t => t.Group_GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chats", "SenderId", "dbo.Accounts");
            DropForeignKey("dbo.Chats", "ReceiverId", "dbo.Accounts");
            DropForeignKey("dbo.Messages", "ChatId", "dbo.Chats");
            DropForeignKey("dbo.Groups", "GroupOwnerAccountId", "dbo.Accounts");
            DropForeignKey("dbo.AccountGroups", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.AccountGroups", "Account_AccountId", "dbo.Accounts");
            DropForeignKey("dbo.GroupMessages", "SenderId", "dbo.Accounts");
            DropForeignKey("dbo.GroupMessages", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Contacts", "ContactOwnerId", "dbo.Accounts");
            DropForeignKey("dbo.Contacts", "OwnerAccountId", "dbo.Accounts");
            DropIndex("dbo.AccountGroups", new[] { "Group_GroupId" });
            DropIndex("dbo.AccountGroups", new[] { "Account_AccountId" });
            DropIndex("dbo.Messages", new[] { "ChatId" });
            DropIndex("dbo.Chats", new[] { "ReceiverId" });
            DropIndex("dbo.Chats", new[] { "SenderId" });
            DropIndex("dbo.Groups", new[] { "GroupOwnerAccountId" });
            DropIndex("dbo.GroupMessages", new[] { "SenderId" });
            DropIndex("dbo.GroupMessages", new[] { "GroupId" });
            DropIndex("dbo.Contacts", new[] { "ContactOwnerId" });
            DropIndex("dbo.Contacts", new[] { "OwnerAccountId" });
            DropTable("dbo.AccountGroups");
            DropTable("dbo.Messages");
            DropTable("dbo.Chats");
            DropTable("dbo.Groups");
            DropTable("dbo.GroupMessages");
            DropTable("dbo.Contacts");
            DropTable("dbo.Accounts");
        }
    }
}
