using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace KiotlogDBF.Migrations
{
    public static class Extensions
    {
        public static OperationBuilder<SqlOperation> CreateUser(
            this MigrationBuilder migrationBuilder,
            string user,
            string password)
        {
            // https://stackoverflow.com/questions/8092086/create-postgresql-role-user-if-it-doesnt-exist#8099557
            string query = @"
                    DO
                    $do$
                    BEGIN
                    IF NOT EXISTS (
                        SELECT
                        FROM   pg_catalog.pg_roles
                        WHERE  rolname = '{0}') THEN

                        CREATE USER {0} WITH LOGIN NOSUPERUSER INHERIT NOCREATEDB NOCREATEROLE NOREPLICATION PASSWORD '{1}';
                    END IF;
                    END
                    $do$;";

            return migrationBuilder.Sql(string.Format(query, user, password));

        }

        public static OperationBuilder<SqlOperation> CreateRole(
            this MigrationBuilder migrationBuilder,
            string role)
        {
            string query = @"
                    DO
                    $do$
                    BEGIN
                    IF NOT EXISTS (
                        SELECT
                        FROM   pg_catalog.pg_roles
                        WHERE  rolname = '{0}') THEN

                        CREATE ROLE {0} WITH NOLOGIN NOSUPERUSER INHERIT NOCREATEDB NOCREATEROLE NOREPLICATION;
                    END IF;
                    END
                    $do$;";

            return migrationBuilder.Sql(string.Format(query, role));

        }

        public static OperationBuilder<SqlOperation> GrantRoleToUser(
            this MigrationBuilder migrationBuilder,
            string user,
            string role)
            => migrationBuilder.Sql($"GRANT {role} TO {user};");

        public static OperationBuilder<SqlOperation> SetOwner(
            this MigrationBuilder migrationBuilder,
            string table,
            string role)
            => migrationBuilder.Sql($"ALTER TABLE {table} OWNER TO {role};");
        public static OperationBuilder<SqlOperation> GrantSelect(
            this MigrationBuilder migrationBuilder,
            string table,
            string role)
            => migrationBuilder.Sql($"GRANT SELECT ON TABLE {table} TO {role};");
    }
}
