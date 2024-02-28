using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeFirstBD.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    CategoriaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GradosA = table.Column<int>(type: "int", nullable: false),
                    EsAlcoholica = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.CategoriaID);
                });

            migrationBuilder.CreateTable(
                name: "Cerveza",
                columns: table => new
                {
                    CervezaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CervezaNombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cerveza", x => x.CervezaID);
                    table.ForeignKey(
                        name: "FK_Cerveza_Categoria_CategoriaID",
                        column: x => x.CategoriaID,
                        principalTable: "Categoria",
                        principalColumn: "CategoriaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jugo",
                columns: table => new
                {
                    JugoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JugoNombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jugo", x => x.JugoID);
                    table.ForeignKey(
                        name: "FK_Jugo_Categoria_CategoriaID",
                        column: x => x.CategoriaID,
                        principalTable: "Categoria",
                        principalColumn: "CategoriaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vino",
                columns: table => new
                {
                    VinoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VinoNombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vino", x => x.VinoID);
                    table.ForeignKey(
                        name: "FK_Vino_Categoria_CategoriaID",
                        column: x => x.CategoriaID,
                        principalTable: "Categoria",
                        principalColumn: "CategoriaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cerveza_CategoriaID",
                table: "Cerveza",
                column: "CategoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_Jugo_CategoriaID",
                table: "Jugo",
                column: "CategoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_Vino_CategoriaID",
                table: "Vino",
                column: "CategoriaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cerveza");

            migrationBuilder.DropTable(
                name: "Jugo");

            migrationBuilder.DropTable(
                name: "Vino");

            migrationBuilder.DropTable(
                name: "Categoria");
        }
    }
}
