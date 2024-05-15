using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VueApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class addaccountuserroletablesandestablishrelationshipbetweenallofthem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "salt",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "firstName",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "age",
                table: "Users",
                newName: "Age");

            migrationBuilder.RenameColumn(
                name: "school",
                table: "Users",
                newName: "PhoneNum");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Users",
                newName: "Birthday");

            migrationBuilder.RenameColumn(
                name: "hashPassword",
                table: "Accounts",
                newName: "HashPassword");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Accounts",
                newName: "Username");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeType",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ManagerName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeType",
                table: "Users",
                column: "EmployeeType");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Accounts_Email",
                table: "Users",
                column: "Email",
                principalTable: "Accounts",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_EmployeeType",
                table: "Users",
                column: "EmployeeType",
                principalTable: "Roles",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Accounts_Email",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_EmployeeType",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmployeeType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmployeeType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ManagerName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "firstName");

            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Users",
                newName: "age");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "PhoneNum",
                table: "Users",
                newName: "school");

            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "Users",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "HashPassword",
                table: "Accounts",
                newName: "hashPassword");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Accounts",
                newName: "username");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<string>(
                name: "salt",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "ID");
        }
    }
}
