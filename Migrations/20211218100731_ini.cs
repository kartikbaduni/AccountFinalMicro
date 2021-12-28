using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AccountMicroservice.Migrations
{
    public partial class ini : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    AccountCreationDate = table.Column<DateTime>(nullable: false),
                    AccountType = table.Column<string>(nullable: false),
                    CurrentBalance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    TransactionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    ValueDate = table.Column<DateTime>(nullable: false),
                    Reference = table.Column<string>(nullable: false),
                    Deposit = table.Column<double>(nullable: false),
                    Withdrawal = table.Column<double>(nullable: false),
                    ClosingBalance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Statements_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Statements_AccountId",
                table: "Statements",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statements");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
