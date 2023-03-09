using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieProductionApp.Migrations
{
    /// <inheritdoc />
    public partial class productionInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    ActorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.ActorId);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "ProductionStudios",
                columns: table => new
                {
                    ProductionStudioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionStudios", x => x.ProductionStudioId);
                });

            migrationBuilder.CreateTable(
                name: "StreamCompanies",
                columns: table => new
                {
                    StreamCompanyInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    webApiUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    challengeUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    verificationStatus = table.Column<bool>(type: "bit", nullable: false),
                    StreamGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamCompanies", x => x.StreamCompanyInfoId);
                    table.UniqueConstraint("AK_StreamCompanies_Name_webApiUrl", x => new { x.Name, x.webApiUrl });
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.MovieId);
                    table.ForeignKey(
                        name: "FK_Movies_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Castings",
                columns: table => new
                {
                    ActorId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Castings", x => new { x.ActorId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_Castings_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "ActorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Castings_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieApiData",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    ProductionStudioId = table.Column<int>(type: "int", nullable: false),
                    TimeOfOffer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StreamCompanyInfoId = table.Column<int>(type: "int", nullable: true),
                    Availability = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieApiData", x => new { x.MovieId, x.ProductionStudioId });
                    table.ForeignKey(
                        name: "FK_MovieApiData_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieApiData_ProductionStudios_ProductionStudioId",
                        column: x => x.ProductionStudioId,
                        principalTable: "ProductionStudios",
                        principalColumn: "ProductionStudioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieApiData_StreamCompanies_StreamCompanyInfoId",
                        column: x => x.StreamCompanyInfoId,
                        principalTable: "StreamCompanies",
                        principalColumn: "StreamCompanyInfoId");
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Review_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "ActorId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Humphrey", "Bogart" },
                    { 2, "Ingrid", "Bergman" },
                    { 3, "Keanu", "Reeves" },
                    { 4, "Carrie-Anne", "Moss" },
                    { 5, "John", "Travolta" },
                    { 6, "Uma", "Thurman" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreId", "Name" },
                values: new object[,]
                {
                    { "A", "Action" },
                    { "C", "Comedy" },
                    { "D", "Drama" },
                    { "H", "Horror" },
                    { "M", "Musical" },
                    { "R", "RomCom" },
                    { "S", "SciFi" }
                });

            migrationBuilder.InsertData(
                table: "ProductionStudios",
                columns: new[] { "ProductionStudioId", "Name" },
                values: new object[] { 2, "MPC Productions" });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "MovieId", "GenreId", "Name", "Year" },
                values: new object[,]
                {
                    { 1, "D", "Casablanca", 1942 },
                    { 2, "A", "The Matrix", 1998 },
                    { 3, "C", "Pulp Fiction", 1992 }
                });

            migrationBuilder.InsertData(
                table: "Castings",
                columns: new[] { "ActorId", "MovieId", "Role" },
                values: new object[,]
                {
                    { 1, 1, "Rick Blaine" },
                    { 2, 1, "Ilsa Lund" },
                    { 3, 2, "Neo" },
                    { 4, 2, "Trinity" },
                    { 5, 3, "Vincet Vega" },
                    { 6, 3, "Mia Wallace" }
                });

            migrationBuilder.InsertData(
                table: "MovieApiData",
                columns: new[] { "MovieId", "ProductionStudioId", "Availability", "StreamCompanyInfoId", "TimeOfOffer" },
                values: new object[,]
                {
                    { 1, 2, true, null, new DateTime(2010, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, false, null, new DateTime(2012, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 2, true, null, new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Review",
                columns: new[] { "ReviewId", "Comments", "MovieId", "Rating" },
                values: new object[,]
                {
                    { 1, "A classic!", 1, 5 },
                    { 2, "They should have gotten together in the end!", 1, 3 },
                    { 3, "Too slow of a pace", 1, 3 },
                    { 4, "Based on Descarte's \"brain in a vat\" thought experiment", 2, 4 },
                    { 5, "Very philosophical", 2, 3 },
                    { 6, "Very violent but also very funny and clever", 3, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Castings_MovieId",
                table: "Castings",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieApiData_ProductionStudioId",
                table: "MovieApiData",
                column: "ProductionStudioId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieApiData_StreamCompanyInfoId",
                table: "MovieApiData",
                column: "StreamCompanyInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GenreId",
                table: "Movies",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionStudios_Name",
                table: "ProductionStudios",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_MovieId",
                table: "Review",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamCompanies_StreamGUID",
                table: "StreamCompanies",
                column: "StreamGUID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Castings");

            migrationBuilder.DropTable(
                name: "MovieApiData");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "ProductionStudios");

            migrationBuilder.DropTable(
                name: "StreamCompanies");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
