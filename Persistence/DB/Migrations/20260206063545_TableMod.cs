using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.DB.Migrations
{
    /// <inheritdoc />
    public partial class TableMod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineBatches_Medicines_Id",
                table: "MedicineBatches");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBatches_MedicineId",
                table: "MedicineBatches",
                column: "MedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineBatches_Medicines_MedicineId",
                table: "MedicineBatches",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineBatches_Medicines_MedicineId",
                table: "MedicineBatches");

            migrationBuilder.DropIndex(
                name: "IX_MedicineBatches_MedicineId",
                table: "MedicineBatches");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineBatches_Medicines_Id",
                table: "MedicineBatches",
                column: "Id",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
