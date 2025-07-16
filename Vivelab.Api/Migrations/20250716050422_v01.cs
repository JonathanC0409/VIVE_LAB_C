using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Vivelab.Api.Migrations
{
    /// <inheritdoc />
    public partial class v01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    UserCount = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Bibliography = table.Column<string>(type: "text", nullable: true),
                    Balance = table.Column<double>(type: "double precision", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CoverUrl = table.Column<string>(type: "text", nullable: true),
                    ArtistCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Albums_Users_ArtistCode",
                        column: x => x.ArtistCode,
                        principalTable: "Users",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Playlists_Users_UserCode",
                        column: x => x.UserCode,
                        principalTable: "Users",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    PlanCode = table.Column<int>(type: "integer", nullable: false),
                    UserCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Plans_PlanCode",
                        column: x => x.PlanCode,
                        principalTable: "Plans",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_UserCode",
                        column: x => x.UserCode,
                        principalTable: "Users",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    FileUrl = table.Column<string>(type: "text", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalPlays = table.Column<int>(type: "integer", nullable: false),
                    CoverUrl = table.Column<string>(type: "text", nullable: true),
                    ArtistCode = table.Column<int>(type: "integer", nullable: false),
                    AlbumCode = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Songs_Albums_AlbumCode",
                        column: x => x.AlbumCode,
                        principalTable: "Albums",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Songs_Users_ArtistCode",
                        column: x => x.ArtistCode,
                        principalTable: "Users",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersSubscriptions",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriptionCode = table.Column<int>(type: "integer", nullable: false),
                    UserCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersSubscriptions", x => x.Code);
                    table.ForeignKey(
                        name: "FK_UsersSubscriptions_Subscriptions_SubscriptionCode",
                        column: x => x.SubscriptionCode,
                        principalTable: "Subscriptions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersSubscriptions_Users_UserCode",
                        column: x => x.UserCode,
                        principalTable: "Users",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistSongs",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlaylistCode = table.Column<int>(type: "integer", nullable: false),
                    SongCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSongs", x => x.Code);
                    table.ForeignKey(
                        name: "FK_PlaylistSongs_Playlists_PlaylistCode",
                        column: x => x.PlaylistCode,
                        principalTable: "Playlists",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistSongs_Songs_SongCode",
                        column: x => x.SongCode,
                        principalTable: "Songs",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ArtistCode",
                table: "Albums",
                column: "ArtistCode");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UserCode",
                table: "Playlists",
                column: "UserCode");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_PlaylistCode",
                table: "PlaylistSongs",
                column: "PlaylistCode");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_SongCode",
                table: "PlaylistSongs",
                column: "SongCode");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_AlbumCode",
                table: "Songs",
                column: "AlbumCode");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_ArtistCode",
                table: "Songs",
                column: "ArtistCode");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PlanCode",
                table: "Subscriptions",
                column: "PlanCode");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserCode",
                table: "Subscriptions",
                column: "UserCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersSubscriptions_SubscriptionCode",
                table: "UsersSubscriptions",
                column: "SubscriptionCode");

            migrationBuilder.CreateIndex(
                name: "IX_UsersSubscriptions_UserCode",
                table: "UsersSubscriptions",
                column: "UserCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistSongs");

            migrationBuilder.DropTable(
                name: "UsersSubscriptions");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
