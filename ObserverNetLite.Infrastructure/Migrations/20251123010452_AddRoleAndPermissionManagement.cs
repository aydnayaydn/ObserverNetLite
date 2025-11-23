using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ObserverNetLite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleAndPermissionManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Icon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Route = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MenuId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuPermissions_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "Icon", "IsActive", "Name", "Order", "ParentId", "Route", "Title" },
                values: new object[,]
                {
                    { new Guid("40000001-0000-0000-0000-000000000001"), "dashboard", true, "dashboard", 1, null, "/dashboard", "Dashboard" },
                    { new Guid("40000002-0000-0000-0000-000000000002"), "people", true, "users", 2, null, "/users", "Users" },
                    { new Guid("40000003-0000-0000-0000-000000000003"), "shield", true, "roles", 3, null, "/roles", "Roles" },
                    { new Guid("40000004-0000-0000-0000-000000000004"), "settings", true, "settings", 5, null, "/settings", "Settings" },
                    { new Guid("40000005-0000-0000-0000-000000000005"), "menu", true, "menus", 4, null, "/menus", "Menus" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Category", "Code", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("10000001-0000-0000-0000-000000000001"), "User", "USER_VIEW", "View user list and details", "View Users" },
                    { new Guid("10000002-0000-0000-0000-000000000002"), "User", "USER_CREATE", "Create new users", "Create User" },
                    { new Guid("10000003-0000-0000-0000-000000000003"), "User", "USER_EDIT", "Edit existing users", "Edit User" },
                    { new Guid("10000004-0000-0000-0000-000000000004"), "User", "USER_DELETE", "Delete users", "Delete User" },
                    { new Guid("20000001-0000-0000-0000-000000000001"), "Role", "ROLE_VIEW", "View role list and details", "View Roles" },
                    { new Guid("20000002-0000-0000-0000-000000000002"), "Role", "ROLE_CREATE", "Create new roles", "Create Role" },
                    { new Guid("20000003-0000-0000-0000-000000000003"), "Role", "ROLE_EDIT", "Edit existing roles", "Edit Role" },
                    { new Guid("20000004-0000-0000-0000-000000000004"), "Role", "ROLE_DELETE", "Delete roles", "Delete Role" },
                    { new Guid("30000001-0000-0000-0000-000000000001"), "Menu", "MENU_VIEW", "View menu list and details", "View Menus" },
                    { new Guid("30000002-0000-0000-0000-000000000002"), "Menu", "MENU_CREATE", "Create new menus", "Create Menu" },
                    { new Guid("30000003-0000-0000-0000-000000000003"), "Menu", "MENU_EDIT", "Edit existing menus", "Edit Menu" },
                    { new Guid("30000004-0000-0000-0000-000000000004"), "Menu", "MENU_DELETE", "Delete menus", "Delete Menu" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Administrator role with full access", true, "admin" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Guest role with limited access", true, "guest" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                column: "RoleId",
                value: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9e225865-a24d-4543-a6c6-9443d048cdb9"),
                columns: new[] { "Email", "Password", "RoleId", "UserName" },
                values: new object[] { "guest@observernetlite.com", "fcf41657f02f88137a1bcf068a32c0a3", new Guid("22222222-2222-2222-2222-222222222222"), "guest" });

            migrationBuilder.InsertData(
                table: "MenuPermissions",
                columns: new[] { "Id", "MenuId", "PermissionId" },
                values: new object[,]
                {
                    { new Guid("60000001-0000-0000-0000-000000000001"), new Guid("40000001-0000-0000-0000-000000000001"), new Guid("30000001-0000-0000-0000-000000000001") },
                    { new Guid("60000002-0000-0000-0000-000000000002"), new Guid("40000002-0000-0000-0000-000000000002"), new Guid("30000001-0000-0000-0000-000000000001") },
                    { new Guid("60000003-0000-0000-0000-000000000003"), new Guid("40000003-0000-0000-0000-000000000003"), new Guid("30000001-0000-0000-0000-000000000001") },
                    { new Guid("60000004-0000-0000-0000-000000000004"), new Guid("40000005-0000-0000-0000-000000000005"), new Guid("30000001-0000-0000-0000-000000000001") },
                    { new Guid("60000005-0000-0000-0000-000000000005"), new Guid("40000004-0000-0000-0000-000000000004"), new Guid("30000001-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("50000001-0000-0000-0000-000000000001"), new Guid("10000001-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("50000002-0000-0000-0000-000000000002"), new Guid("10000002-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("50000003-0000-0000-0000-000000000003"), new Guid("10000003-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("50000004-0000-0000-0000-000000000004"), new Guid("10000004-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("50000005-0000-0000-0000-000000000005"), new Guid("20000001-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("50000006-0000-0000-0000-000000000006"), new Guid("20000002-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("50000007-0000-0000-0000-000000000007"), new Guid("20000003-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("50000008-0000-0000-0000-000000000008"), new Guid("20000004-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("50000009-0000-0000-0000-000000000009"), new Guid("30000001-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("5000000a-0000-0000-0000-00000000000a"), new Guid("30000002-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("5000000b-0000-0000-0000-00000000000b"), new Guid("30000003-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("5000000c-0000-0000-0000-00000000000c"), new Guid("30000004-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("5000000d-0000-0000-0000-00000000000d"), new Guid("10000001-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("5000000e-0000-0000-0000-00000000000e"), new Guid("30000001-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermissions_MenuId_PermissionId",
                table: "MenuPermissions",
                columns: new[] { "MenuId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermissions_PermissionId",
                table: "MenuPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_ParentId",
                table: "Menus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Code",
                table: "Permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId_PermissionId",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "MenuPermissions");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                column: "Role",
                value: "admin");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9e225865-a24d-4543-a6c6-9443d048cdb9"),
                columns: new[] { "Email", "Password", "Role", "UserName" },
                values: new object[] { "user@observernetlite.com", "6ad14ba9986e3615423dfca256d04e3f", "user", "user" });
        }
    }
}
