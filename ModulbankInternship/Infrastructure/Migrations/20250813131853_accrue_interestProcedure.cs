using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModulbankInternship.Migrations
{
    /// <inheritdoc />
    public partial class accrue_interestProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION accrue_interest(account_id UUID)
RETURNS void AS $$
DECLARE
    current_balance NUMERIC;
    interest_rate NUMERIC;
    interest_amount NUMERIC;
BEGIN
    SELECT balance, rate INTO current_balance, interest_rate
    FROM accounts
    WHERE id = account_id;

    IF current_balance IS NULL THEN
        RAISE EXCEPTION 'Account % not found', account_id;
    END IF;

    interest_amount := current_balance * interest_rate / 365;

    UPDATE accounts
    SET balance = balance + interest_amount
    WHERE id = account_id;
END;
$$ LANGUAGE plpgsql;
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS accrue_interest(UUID);");
        }
    }
}
