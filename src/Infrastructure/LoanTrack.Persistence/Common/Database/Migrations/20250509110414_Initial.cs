using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanTrack.Persistence.Common.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "loantrack");

            migrationBuilder.CreateSequence<int>(
                name: "code_seq_start_one_inc",
                schema: "loantrack");

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "loantrack",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    identity_id = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FinanceJournals",
                schema: "loantrack",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    journal_date = table.Column<DateOnly>(type: "date", nullable: false),
                    journal_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    amount = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    reference_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    reference_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_finance_journals", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ListValues",
                schema: "loantrack",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    list_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_list_values", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LoanSchemes",
                schema: "loantrack",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    interest_type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    interest_rate = table.Column<double>(type: "double precision", precision: 5, scale: 2, nullable: false),
                    minimum_amount = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    maximum_amount = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    repayment_periods_in_months = table.Column<int>(type: "integer", nullable: false),
                    processing_fee = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    insurance_amount = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    late_payment_penalty = table.Column<double>(type: "double precision", precision: 5, scale: 2, nullable: false),
                    is_secured_loan = table.Column<bool>(type: "boolean", nullable: false),
                    collateral_type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    has_fixed_interest_rate = table.Column<bool>(type: "boolean", nullable: false),
                    is_government_subsidized = table.Column<bool>(type: "boolean", nullable: false),
                    eligible_borrower_types = table.Column<string>(type: "jsonb", nullable: true),
                    allowed_loan_purposes = table.Column<string>(type: "jsonb", nullable: true),
                    requires_guarantor = table.Column<bool>(type: "boolean", nullable: false),
                    grace_period_in_months = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loan_schemes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Centers",
                schema: "loantrack",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    center_leader_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_centers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerGroups",
                schema: "loantrack",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    center_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_groups", x => x.id);
                    table.ForeignKey(
                        name: "fk_customer_groups_centers_center_id",
                        column: x => x.center_id,
                        principalSchema: "loantrack",
                        principalTable: "Centers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "loantrack",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('loantrack.code_seq_start_one_inc')"),
                    nic = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    grama_niladhari_id = table.Column<Guid>(type: "uuid", nullable: true),
                    ds_division_id = table.Column<Guid>(type: "uuid", nullable: true),
                    district_id = table.Column<Guid>(type: "uuid", nullable: true),
                    province_id = table.Column<Guid>(type: "uuid", nullable: true),
                    center_id = table.Column<Guid>(type: "uuid", nullable: true),
                    group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    occupation = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    work_address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    bank_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    bank_branch = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    bank_account_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    account_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                    table.ForeignKey(
                        name: "fk_customers_centers_center_id",
                        column: x => x.center_id,
                        principalSchema: "loantrack",
                        principalTable: "Centers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_customers_customer_groups_group_id",
                        column: x => x.group_id,
                        principalSchema: "loantrack",
                        principalTable: "CustomerGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_customers_list_values_district_id",
                        column: x => x.district_id,
                        principalSchema: "loantrack",
                        principalTable: "ListValues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_customers_list_values_ds_division_id",
                        column: x => x.ds_division_id,
                        principalSchema: "loantrack",
                        principalTable: "ListValues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_customers_list_values_grama_niladhari_id",
                        column: x => x.grama_niladhari_id,
                        principalSchema: "loantrack",
                        principalTable: "ListValues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_customers_list_values_province_id",
                        column: x => x.province_id,
                        principalSchema: "loantrack",
                        principalTable: "ListValues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                schema: "loantrack",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    loan_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    loan_scheme_id = table.Column<Guid>(type: "uuid", nullable: true),
                    loan_officer = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    loan_amount = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    interest_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    interest_rate = table.Column<double>(type: "double precision", precision: 5, scale: 2, nullable: false),
                    installment_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    duration_in_interest_units = table.Column<int>(type: "integer", nullable: false),
                    repayment_durations = table.Column<int>(type: "integer", nullable: false),
                    installment_amount = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    issuance_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    next_installment_date = table.Column<DateOnly>(type: "date", nullable: true),
                    loan_disbursement_method = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    loan_repayment_method = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    guarantors_information = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    loan_status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    closed_date = table.Column<DateOnly>(type: "date", nullable: true),
                    total_amount_payable = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    paid_amount = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false, defaultValue: 0.0),
                    processing_fee = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false, defaultValue: 0.0),
                    insurance_amount = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false, defaultValue: 0.0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loans", x => x.id);
                    table.ForeignKey(
                        name: "fk_loans_customers_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "loantrack",
                        principalTable: "Customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_loans_loan_schemes_loan_scheme_id",
                        column: x => x.loan_scheme_id,
                        principalSchema: "loantrack",
                        principalTable: "LoanSchemes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LoanInstallments",
                schema: "loantrack",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    loan_id = table.Column<Guid>(type: "uuid", nullable: false),
                    installment_number = table.Column<int>(type: "integer", nullable: false),
                    installment_date = table.Column<DateOnly>(type: "date", nullable: false),
                    installment_amount = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    is_paid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_delayed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    delayed_days = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_penalty_applied = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    penalty_amount = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false, defaultValue: 0.0),
                    penalty_reason = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    payment_date = table.Column<DateOnly>(type: "date", nullable: true),
                    amount_paid = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false, defaultValue: 0.0),
                    payment_method = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    payment_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loan_installments", x => x.id);
                    table.ForeignKey(
                        name: "fk_loan_installments_loans_loan_id",
                        column: x => x.loan_id,
                        principalSchema: "loantrack",
                        principalTable: "Loans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "loantrack",
                table: "Employees",
                columns: new[] { "id", "created_at", "created_by", "deleted_at", "email", "first_name", "identity_id", "is_active", "is_deleted", "last_name", "role", "updated_at", "updated_by" },
                values: new object[] { new Guid("3d08e0d8-d32a-4220-b7f5-6e3b053adb58"), new DateTime(2025, 5, 9, 11, 4, 13, 594, DateTimeKind.Utc).AddTicks(7967), null, null, "admin@email.com", "System", "09c16865-ee5a-466f-aff4-acbd5eaf8dd8", true, false, "Admin", "Admin", null, null });

            migrationBuilder.CreateIndex(
                name: "ix_centers_center_leader_id",
                schema: "loantrack",
                table: "Centers",
                column: "center_leader_id");

            migrationBuilder.CreateIndex(
                name: "ix_centers_name",
                schema: "loantrack",
                table: "Centers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customer_groups_center_id",
                schema: "loantrack",
                table: "CustomerGroups",
                column: "center_id");

            migrationBuilder.CreateIndex(
                name: "ix_customer_groups_name_center_id",
                schema: "loantrack",
                table: "CustomerGroups",
                columns: new[] { "name", "center_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_center_id",
                schema: "loantrack",
                table: "Customers",
                column: "center_id");

            migrationBuilder.CreateIndex(
                name: "ix_customers_code",
                schema: "loantrack",
                table: "Customers",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_district_id",
                schema: "loantrack",
                table: "Customers",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "ix_customers_ds_division_id",
                schema: "loantrack",
                table: "Customers",
                column: "ds_division_id");

            migrationBuilder.CreateIndex(
                name: "ix_customers_grama_niladhari_id",
                schema: "loantrack",
                table: "Customers",
                column: "grama_niladhari_id");

            migrationBuilder.CreateIndex(
                name: "ix_customers_group_id",
                schema: "loantrack",
                table: "Customers",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "ix_customers_nic",
                schema: "loantrack",
                table: "Customers",
                column: "nic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_province_id",
                schema: "loantrack",
                table: "Customers",
                column: "province_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_email",
                schema: "loantrack",
                table: "Employees",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_identity_id",
                schema: "loantrack",
                table: "Employees",
                column: "identity_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_is_active",
                schema: "loantrack",
                table: "Employees",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_list_values_list_type_description_parent_id",
                schema: "loantrack",
                table: "ListValues",
                columns: new[] { "list_type", "description", "parent_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_loan_installments_loan_id_is_paid",
                schema: "loantrack",
                table: "LoanInstallments",
                columns: new[] { "loan_id", "is_paid" });

            migrationBuilder.CreateIndex(
                name: "ix_loans_customer_id",
                schema: "loantrack",
                table: "Loans",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_loans_loan_number",
                schema: "loantrack",
                table: "Loans",
                column: "loan_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_loans_loan_scheme_id",
                schema: "loantrack",
                table: "Loans",
                column: "loan_scheme_id");

            migrationBuilder.CreateIndex(
                name: "ix_loan_schemes_is_active",
                schema: "loantrack",
                table: "LoanSchemes",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_loan_schemes_name",
                schema: "loantrack",
                table: "LoanSchemes",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_centers_customers_center_leader_id",
                schema: "loantrack",
                table: "Centers",
                column: "center_leader_id",
                principalSchema: "loantrack",
                principalTable: "Customers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_centers_customers_center_leader_id",
                schema: "loantrack",
                table: "Centers");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "loantrack");

            migrationBuilder.DropTable(
                name: "FinanceJournals",
                schema: "loantrack");

            migrationBuilder.DropTable(
                name: "LoanInstallments",
                schema: "loantrack");

            migrationBuilder.DropTable(
                name: "Loans",
                schema: "loantrack");

            migrationBuilder.DropTable(
                name: "LoanSchemes",
                schema: "loantrack");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "loantrack");

            migrationBuilder.DropTable(
                name: "CustomerGroups",
                schema: "loantrack");

            migrationBuilder.DropTable(
                name: "ListValues",
                schema: "loantrack");

            migrationBuilder.DropTable(
                name: "Centers",
                schema: "loantrack");

            migrationBuilder.DropSequence(
                name: "code_seq_start_one_inc",
                schema: "loantrack");
        }
    }
}
