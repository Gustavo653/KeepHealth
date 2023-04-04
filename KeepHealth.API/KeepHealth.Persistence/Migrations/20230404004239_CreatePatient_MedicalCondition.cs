using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeepHealth.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreatePatient_MedicalCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalCondition_Patient_PatientId",
                table: "MedicalCondition");

            migrationBuilder.DropIndex(
                name: "IX_MedicalCondition_PatientId",
                table: "MedicalCondition");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "MedicalCondition");

            migrationBuilder.CreateTable(
                name: "Patient_MedicalConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    MedicalConditionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_MedicalConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patient_MedicalConditions_MedicalCondition_MedicalConditionId",
                        column: x => x.MedicalConditionId,
                        principalTable: "MedicalCondition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_MedicalConditions_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patient_MedicalConditions_MedicalConditionId",
                table: "Patient_MedicalConditions",
                column: "MedicalConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_MedicalConditions_PatientId",
                table: "Patient_MedicalConditions",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patient_MedicalConditions");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "MedicalCondition",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalCondition_PatientId",
                table: "MedicalCondition",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalCondition_Patient_PatientId",
                table: "MedicalCondition",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id");
        }
    }
}
