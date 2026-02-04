using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.DB.Migrations
{
    /// <inheritdoc />
    public partial class ModTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_Medicines_MedicineId",
                table: "InvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineBatches_Medicines_BatchNo",
                table: "MedicineBatches");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_SKU",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Medicines");

            migrationBuilder.RenameColumn(
                name: "BatchNo",
                table: "MedicineBatches",
                newName: "No");

            migrationBuilder.RenameIndex(
                name: "IX_MedicineBatches_BatchNo_MedicineId",
                table: "MedicineBatches",
                newName: "IX_MedicineBatches_No_MedicineId");

            migrationBuilder.RenameColumn(
                name: "MedicineId",
                table: "InvoiceItems",
                newName: "MedicineBatchId");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "InvoiceItems",
                newName: "MedicineBatchNo");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItems_MedicineId",
                table: "InvoiceItems",
                newName: "IX_InvoiceItems_MedicineBatchId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItems_InvoiceId_MedicineId",
                table: "InvoiceItems",
                newName: "IX_InvoiceItems_InvoiceId_MedicineBatchId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerPhone",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNo",
                table: "InvoiceItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_MedicineBatches_MedicineBatchId",
                table: "InvoiceItems",
                column: "MedicineBatchId",
                principalTable: "MedicineBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineBatches_Medicines_Id",
                table: "MedicineBatches",
                column: "Id",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_MedicineBatches_MedicineBatchId",
                table: "InvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineBatches_Medicines_Id",
                table: "MedicineBatches");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CustomerPhone",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "InvoiceNo",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "InvoiceItems");

            migrationBuilder.RenameColumn(
                name: "No",
                table: "MedicineBatches",
                newName: "BatchNo");

            migrationBuilder.RenameIndex(
                name: "IX_MedicineBatches_No_MedicineId",
                table: "MedicineBatches",
                newName: "IX_MedicineBatches_BatchNo_MedicineId");

            migrationBuilder.RenameColumn(
                name: "MedicineBatchNo",
                table: "InvoiceItems",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "MedicineBatchId",
                table: "InvoiceItems",
                newName: "MedicineId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItems_MedicineBatchId",
                table: "InvoiceItems",
                newName: "IX_InvoiceItems_MedicineId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItems_InvoiceId_MedicineBatchId",
                table: "InvoiceItems",
                newName: "IX_InvoiceItems_InvoiceId_MedicineId");

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Medicines",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Total",
                table: "Invoices",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_SKU",
                table: "Medicines",
                column: "SKU",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_Medicines_MedicineId",
                table: "InvoiceItems",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineBatches_Medicines_BatchNo",
                table: "MedicineBatches",
                column: "BatchNo",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
