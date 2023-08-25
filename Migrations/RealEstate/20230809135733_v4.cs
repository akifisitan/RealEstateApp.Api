using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Api.Migrations.RealEstate
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyStatuses_StatusId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyTypes_TypeId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_TypeId",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "PropertyImages",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Properties",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Properties",
                newName: "PropertyTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_StatusId",
                table: "Properties",
                newName: "IX_Properties_PropertyTypeId");

            migrationBuilder.AddColumn<int>(
                name: "PropertyStatusId",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyStatusId",
                table: "Properties",
                column: "PropertyStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyStatuses_PropertyStatusId",
                table: "Properties",
                column: "PropertyStatusId",
                principalTable: "PropertyStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId",
                table: "Properties",
                column: "PropertyTypeId",
                principalTable: "PropertyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyStatuses_PropertyStatusId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyStatusId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PropertyStatusId",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "PropertyImages",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Properties",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "PropertyTypeId",
                table: "Properties",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_PropertyTypeId",
                table: "Properties",
                newName: "IX_Properties_StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_TypeId",
                table: "Properties",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyStatuses_StatusId",
                table: "Properties",
                column: "StatusId",
                principalTable: "PropertyStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyTypes_TypeId",
                table: "Properties",
                column: "TypeId",
                principalTable: "PropertyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
