using Microsoft.EntityFrameworkCore.Migrations;

namespace docker_sandbox_dev_api.Migrations
{
    public partial class adddockerinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DockerContainerId",
                table: "Containers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DockerContainerImage",
                table: "Containers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DockerContainerId",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "DockerContainerImage",
                table: "Containers");
        }
    }
}
