using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TweetBook4.Migrations
{
    public partial class AddTagMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    postId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tags_AspNetUsers_createdBy",
                        column: x => x.createdBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tags_Posts_postId",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_createdBy",
                table: "Tags",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_postId",
                table: "Tags",
                column: "postId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
