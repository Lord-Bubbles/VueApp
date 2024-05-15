using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VueApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "EmployeeType",
                table: "Users",
                newName: "HashPassword");

            migrationBuilder.RenameColumn(
                name: "HashPassword",
                table: "Accounts",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Accounts",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNum",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ManagerName",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "AccountID",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Accounts",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccountID",
                table: "Users",
                column: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Accounts_AccountID",
                table: "Users",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Accounts_AccountID",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AccountID",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AccountID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "HashPassword",
                table: "Users",
                newName: "EmployeeType");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Accounts",
                newName: "HashPassword");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Accounts",
                newName: "Username");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNum",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ManagerName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Username");

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
    }
}
