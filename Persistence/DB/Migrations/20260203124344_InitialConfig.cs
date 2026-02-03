using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.DB.Migrations
{
    /// <inheritdoc />
    public partial class InitialConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_User_CustomerId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_Invoice_InvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_Medicine_MedicineId",
                table: "InvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineBatches_Medicine_BatchNo",
                table: "MedicineBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineDetails_DetailOverview_DetailsId",
                table: "MedicineDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineDetails_Medicine_MedicinesId",
                table: "MedicineDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medicine",
                table: "Medicine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceItem",
                table: "InvoiceItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailOverview",
                table: "DetailOverview");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Medicine",
                newName: "Medicines");

            migrationBuilder.RenameTable(
                name: "InvoiceItem",
                newName: "InvoiceItems");

            migrationBuilder.RenameTable(
                name: "Invoice",
                newName: "Invoices");

            migrationBuilder.RenameTable(
                name: "DetailOverview",
                newName: "DetailOverviews");

            migrationBuilder.RenameIndex(
                name: "IX_Medicine_SKU",
                table: "Medicines",
                newName: "IX_Medicines_SKU");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItem_MedicineId",
                table: "InvoiceItems",
                newName: "IX_InvoiceItems_MedicineId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItem_InvoiceId_MedicineId",
                table: "InvoiceItems",
                newName: "IX_InvoiceItems_InvoiceId_MedicineId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_InvoiceNo",
                table: "Invoices",
                newName: "IX_Invoices_InvoiceNo");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_CustomerId",
                table: "Invoices",
                newName: "IX_Invoices_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medicines",
                table: "Medicines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceItems",
                table: "InvoiceItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailOverviews",
                table: "DetailOverviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_Invoices_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_Medicines_MedicineId",
                table: "InvoiceItems",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Users_CustomerId",
                table: "Invoices",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineBatches_Medicines_BatchNo",
                table: "MedicineBatches",
                column: "BatchNo",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineDetails_DetailOverviews_DetailsId",
                table: "MedicineDetails",
                column: "DetailsId",
                principalTable: "DetailOverviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineDetails_Medicines_MedicinesId",
                table: "MedicineDetails",
                column: "MedicinesId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_Invoices_InvoiceId",
                table: "InvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_Medicines_MedicineId",
                table: "InvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Users_CustomerId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineBatches_Medicines_BatchNo",
                table: "MedicineBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineDetails_DetailOverviews_DetailsId",
                table: "MedicineDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineDetails_Medicines_MedicinesId",
                table: "MedicineDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medicines",
                table: "Medicines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceItems",
                table: "InvoiceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailOverviews",
                table: "DetailOverviews");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Medicines",
                newName: "Medicine");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "Invoice");

            migrationBuilder.RenameTable(
                name: "InvoiceItems",
                newName: "InvoiceItem");

            migrationBuilder.RenameTable(
                name: "DetailOverviews",
                newName: "DetailOverview");

            migrationBuilder.RenameIndex(
                name: "IX_Medicines_SKU",
                table: "Medicine",
                newName: "IX_Medicine_SKU");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_InvoiceNo",
                table: "Invoice",
                newName: "IX_Invoice_InvoiceNo");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoice",
                newName: "IX_Invoice_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItems_MedicineId",
                table: "InvoiceItem",
                newName: "IX_InvoiceItem_MedicineId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItems_InvoiceId_MedicineId",
                table: "InvoiceItem",
                newName: "IX_InvoiceItem_InvoiceId_MedicineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medicine",
                table: "Medicine",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceItem",
                table: "InvoiceItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailOverview",
                table: "DetailOverview",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_User_CustomerId",
                table: "Invoice",
                column: "CustomerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_Invoice_InvoiceId",
                table: "InvoiceItem",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_Medicine_MedicineId",
                table: "InvoiceItem",
                column: "MedicineId",
                principalTable: "Medicine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineBatches_Medicine_BatchNo",
                table: "MedicineBatches",
                column: "BatchNo",
                principalTable: "Medicine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineDetails_DetailOverview_DetailsId",
                table: "MedicineDetails",
                column: "DetailsId",
                principalTable: "DetailOverview",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineDetails_Medicine_MedicinesId",
                table: "MedicineDetails",
                column: "MedicinesId",
                principalTable: "Medicine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
