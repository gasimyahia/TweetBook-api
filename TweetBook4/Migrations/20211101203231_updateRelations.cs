using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TweetBook4.Migrations
{
    public partial class updateRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Tags_TagName",
                table: "PostTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTags",
                table: "PostTags");

            migrationBuilder.DropIndex(
                name: "IX_PostTags_TagName",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "TagName",
                table: "PostTags");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Postid",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TagId",
                table: "PostTags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTags",
                table: "PostTags",
                columns: new[] { "PostId", "TagId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Postid",
                table: "Tags",
                column: "Postid");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_TagId",
                table: "PostTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Tags_TagId",
                table: "PostTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Posts_Postid",
                table: "Tags",
                column: "Postid",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Tags_TagId",
                table: "PostTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Posts_Postid",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Postid",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostTags",
                table: "PostTags");

            migrationBuilder.DropIndex(
                name: "IX_PostTags_TagId",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "id",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Postid",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "PostTags");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Tags",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TagName",
                table: "PostTags",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostTags",
                table: "PostTags",
                columns: new[] { "PostId", "TagName" });

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_TagName",
                table: "PostTags",
                column: "TagName");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Tags_TagName",
                table: "PostTags",
                column: "TagName",
                principalTable: "Tags",
                principalColumn: "name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
