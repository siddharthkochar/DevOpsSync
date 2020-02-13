using Microsoft.EntityFrameworkCore.Migrations;

namespace DevOpsSync.WebApp.API.Migrations
{
    public partial class CreateInitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ServiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Actions_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Triggers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ServiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Triggers_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTriggerAction",
                columns: table => new
                {
                    TriggerId = table.Column<int>(nullable: false),
                    ActionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTriggerAction", x => new { x.TriggerId, x.ActionId });
                    table.ForeignKey(
                        name: "FK_ServiceTriggerAction_Actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Actions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceTriggerAction_Triggers_TriggerId",
                        column: x => x.TriggerId,
                        principalTable: "Triggers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Color", "ImageUrl", "Name" },
                values: new object[] { 1, "#00aeef", "https://assets.ifttt.com/images/channels/2107379463/icons/monochrome_large.png", "GitHub" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Color", "ImageUrl", "Name" },
                values: new object[] { 2, "#0da778", "", "Visual Studio Team Service" });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "Id", "Name", "ServiceId" },
                values: new object[] { 1, "Create Issue", 1 });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "Id", "Name", "ServiceId" },
                values: new object[] { 2, "Sample Action 2", 1 });

            migrationBuilder.InsertData(
                table: "Triggers",
                columns: new[] { "Id", "Description", "Name", "ServiceId" },
                values: new object[] { 1, "When something is pushed on", "Push", 1 });

            migrationBuilder.InsertData(
                table: "ServiceTriggerAction",
                columns: new[] { "TriggerId", "ActionId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Actions_ServiceId",
                table: "Actions",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTriggerAction_ActionId",
                table: "ServiceTriggerAction",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_ServiceId",
                table: "Triggers",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceTriggerAction");

            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "Triggers");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
