using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModulbankInternship.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS btree_gist;");
            migrationBuilder.Sql(
                "CREATE INDEX \"IX_Transactions_date_gist\" ON \"Transactions\" USING gist (\"DateTime\" gist_timestamptz_ops);"
            );
            
            migrationBuilder.CreateIndex(
                    name: "IX_Accounts_ownerId",
                    table: "Accounts",
                    column: "OwnerId")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_accountId_date",
                table: "Transactions",
                columns: new[] { "AccountId", "DateTime" });
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_ownerId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_accountId_date",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_date_gist",
                table: "Transactions");

        }
    }
}
