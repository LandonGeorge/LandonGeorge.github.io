using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WCS.Data.Migrations
{
    public partial class AddedSTEMinUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "STEMHighSchoolClubsAndActivitiesHistory",
                table: "StudentProfiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "STEMHighSchoolClubsAndActivitiesHistory",
                table: "StudentProfiles");
        }
    }
}
