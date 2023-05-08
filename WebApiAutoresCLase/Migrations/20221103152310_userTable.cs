using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiAutoresCLase.Migrations
{
    public partial class userTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApellidoMaterno",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApellidoPaterno",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Calle",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Colonia",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "numeroExterior",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "numeroInteriro",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApellidoMaterno",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApellidoPaterno",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Calle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Colonia",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "numeroExterior",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "numeroInteriro",
                table: "AspNetUsers");
        }
    }
}
