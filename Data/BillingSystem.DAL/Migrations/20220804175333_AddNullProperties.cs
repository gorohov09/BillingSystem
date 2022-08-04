using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingSystem.DAL.Migrations
{
    public partial class AddNullProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coin_UserProfile_UserProfileId",
                table: "Coin");

            migrationBuilder.AlterColumn<long>(
                name: "Amount",
                table: "UserProfile",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "UserProfileId",
                table: "Coin",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Coin_UserProfile_UserProfileId",
                table: "Coin",
                column: "UserProfileId",
                principalTable: "UserProfile",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coin_UserProfile_UserProfileId",
                table: "Coin");

            migrationBuilder.AlterColumn<long>(
                name: "Amount",
                table: "UserProfile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserProfileId",
                table: "Coin",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coin_UserProfile_UserProfileId",
                table: "Coin",
                column: "UserProfileId",
                principalTable: "UserProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
