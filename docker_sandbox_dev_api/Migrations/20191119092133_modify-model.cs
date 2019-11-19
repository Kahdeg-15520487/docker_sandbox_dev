using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace docker_sandbox_dev_api.Migrations
{
    public partial class modifymodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Sandboxes",
                columns: table => new
                {
                    SandboxId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    SandboxCreationConfig = table.Column<string>(nullable: true),
                    DockerContainerId = table.Column<string>(nullable: true),
                    DockerContainerPort = table.Column<string>(nullable: true),
                    DockerContainerImage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sandboxes", x => x.SandboxId);
                    table.ForeignKey(
                        name: "FK_Sandboxes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sandboxes_UserId",
                table: "Sandboxes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sandboxes");

            migrationBuilder.DropColumn(
                name: "Port",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    ContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DockerContainerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DockerContainerImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.ContainerId);
                    table.ForeignKey(
                        name: "FK_Containers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Containers_UserId",
                table: "Containers",
                column: "UserId");
        }
    }
}
