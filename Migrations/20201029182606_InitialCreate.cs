using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace QuestionServiceWebApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Owner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reputation = table.Column<int>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    user_type = table.Column<string>(nullable: true),
                    profile_image = table.Column<string>(nullable: true),
                    display_name = table.Column<string>(nullable: true),
                    link = table.Column<string>(nullable: true),
                    accept_rate = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tags = table.Column<List<string>>(nullable: true),
                    ownerId = table.Column<int>(nullable: true),
                    is_answered = table.Column<bool>(nullable: false),
                    view_count = table.Column<int>(nullable: false),
                    answer_count = table.Column<int>(nullable: false),
                    score = table.Column<int>(nullable: false),
                    last_activity_date = table.Column<int>(nullable: false),
                    creation_date = table.Column<int>(nullable: false),
                    last_edit_date = table.Column<int>(nullable: false),
                    question_id = table.Column<int>(nullable: false),
                    content_license = table.Column<string>(nullable: true),
                    link = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true),
                    bounty_amount = table.Column<int>(nullable: true),
                    bounty_closes_date = table.Column<int>(nullable: true),
                    closed_date = table.Column<int>(nullable: true),
                    closed_reason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Owner_ownerId",
                        column: x => x.ownerId,
                        principalTable: "Owner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ownerId",
                table: "Questions",
                column: "ownerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Owner");
        }
    }
}
