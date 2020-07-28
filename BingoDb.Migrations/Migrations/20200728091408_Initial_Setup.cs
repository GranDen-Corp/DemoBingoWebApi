using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BingoDb.Migrations.Migrations
{
    public partial class Initial_Setup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MappingGeoPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeoPointId = table.Column<string>(type: "varchar(256)", nullable: false),
                    GeoPointRedirected = table.Column<bool>(nullable: false),
                    RedirectedGeoPointId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MappingGeoPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BingoPlayerInfos",
                columns: table => new
                {
                    PlayerId = table.Column<string>(type: "varchar(128)", nullable: false),
                    Bingo2dGameInfoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BingoPlayerInfos", x => x.PlayerId);
                });

            migrationBuilder.CreateTable(
                name: "Bingo2dGameInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameName = table.Column<string>(nullable: false),
                    I18nDisplayKey = table.Column<string>(nullable: true),
                    MaxWidth = table.Column<int>(nullable: false),
                    MaxHeight = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTimeOffset>(nullable: false),
                    EndTime = table.Column<DateTimeOffset>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    BingoPlayerInfoPlayerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bingo2dGameInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bingo2dGameInfos_BingoPlayerInfos_BingoPlayerInfoPlayerId",
                        column: x => x.BingoPlayerInfoPlayerId,
                        principalTable: "BingoPlayerInfos",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BingoPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<int>(nullable: true),
                    Y = table.Column<int>(nullable: true),
                    Marked = table.Column<bool>(nullable: true),
                    BelongingGameId = table.Column<int>(nullable: true),
                    BelongingPlayerPlayerId = table.Column<string>(nullable: true),
                    ClearTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BingoPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BingoPoints_Bingo2dGameInfos_BelongingGameId",
                        column: x => x.BelongingGameId,
                        principalTable: "Bingo2dGameInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BingoPoints_BingoPlayerInfos_BelongingPlayerPlayerId",
                        column: x => x.BelongingPlayerPlayerId,
                        principalTable: "BingoPlayerInfos",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PointProjection",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BingoPlayerInfoPlayerId = table.Column<string>(nullable: true),
                    BingoPointFk = table.Column<int>(nullable: false),
                    MappingGeoPointId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointProjection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointProjection_BingoPlayerInfos_BingoPlayerInfoPlayerId",
                        column: x => x.BingoPlayerInfoPlayerId,
                        principalTable: "BingoPlayerInfos",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PointProjection_BingoPoints_BingoPointFk",
                        column: x => x.BingoPointFk,
                        principalTable: "BingoPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointProjection_MappingGeoPoints_MappingGeoPointId",
                        column: x => x.MappingGeoPointId,
                        principalTable: "MappingGeoPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bingo2dGameInfos_BingoPlayerInfoPlayerId",
                table: "Bingo2dGameInfos",
                column: "BingoPlayerInfoPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bingo2dGameInfos_GameName",
                table: "Bingo2dGameInfos",
                column: "GameName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BingoPlayerInfos_Bingo2dGameInfoId",
                table: "BingoPlayerInfos",
                column: "Bingo2dGameInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_BingoPoints_BelongingGameId",
                table: "BingoPoints",
                column: "BelongingGameId");

            migrationBuilder.CreateIndex(
                name: "IX_BingoPoints_BelongingPlayerPlayerId",
                table: "BingoPoints",
                column: "BelongingPlayerPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MappingGeoPoints_GeoPointId",
                table: "MappingGeoPoints",
                column: "GeoPointId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PointProjection_BingoPlayerInfoPlayerId",
                table: "PointProjection",
                column: "BingoPlayerInfoPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PointProjection_BingoPointFk",
                table: "PointProjection",
                column: "BingoPointFk",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PointProjection_MappingGeoPointId",
                table: "PointProjection",
                column: "MappingGeoPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_BingoPlayerInfos_Bingo2dGameInfos_Bingo2dGameInfoId",
                table: "BingoPlayerInfos",
                column: "Bingo2dGameInfoId",
                principalTable: "Bingo2dGameInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bingo2dGameInfos_BingoPlayerInfos_BingoPlayerInfoPlayerId",
                table: "Bingo2dGameInfos");

            migrationBuilder.DropTable(
                name: "PointProjection");

            migrationBuilder.DropTable(
                name: "BingoPoints");

            migrationBuilder.DropTable(
                name: "MappingGeoPoints");

            migrationBuilder.DropTable(
                name: "BingoPlayerInfos");

            migrationBuilder.DropTable(
                name: "Bingo2dGameInfos");
        }
    }
}
