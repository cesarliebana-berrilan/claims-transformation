using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Berrilan.Claims.Core.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                    license = table.Column<int>(type: "integer", nullable: false),
                    is_root = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "customer_id", "email", "is_root", "license", "role" },
                values: new object[] { new Guid("0194efaa-0095-d8fd-7903-976365a009fd"), new Guid("0194efa9-938a-7227-3ece-1c954289b196"), "cliebana@berrilan.com", true, 1, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
