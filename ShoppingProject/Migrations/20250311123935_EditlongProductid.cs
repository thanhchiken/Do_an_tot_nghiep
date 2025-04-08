using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingProject.Migrations
{
    /// <inheritdoc />
    public partial class EditlongProductid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ProductID",
                table: "OrderDetails",
                type: "bigint",
                nullable: false,
                //defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)"/*,*/
                /*oldNullable: true*/);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductID",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
