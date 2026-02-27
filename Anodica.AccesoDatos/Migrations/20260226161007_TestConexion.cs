using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Anodica.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class TestConexion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Insumo",
                columns: table => new
                {
                    InsumoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoInsumo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsumoNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CantidadStock = table.Column<int>(type: "int", nullable: false),
                    CantMinimaStock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insumo", x => x.InsumoID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Insumo");
        }
    }
}
