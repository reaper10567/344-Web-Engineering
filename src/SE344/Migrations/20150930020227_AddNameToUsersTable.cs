using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace SE344.Migrations
{
    public partial class AddNameToUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                isNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Name", table: "AspNetUsers");
        }
    }
}
