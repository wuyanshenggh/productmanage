using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductManageNew.Migrations
{
    public partial class firstMysqlMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prc_ConfigSet",
                columns: table => new
                {
                    ID = table.Column<byte[]>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateOperateUser = table.Column<string>(nullable: true),
                    LastOperateUser = table.Column<string>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<short>(nullable: false),
                    Code = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prc_ConfigSet", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prc_Holiday",
                columns: table => new
                {
                    ID = table.Column<byte[]>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateOperateUser = table.Column<string>(nullable: true),
                    LastOperateUser = table.Column<string>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<short>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prc_Holiday", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prc_UpgradeInfo",
                columns: table => new
                {
                    ID = table.Column<byte[]>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateOperateUser = table.Column<string>(nullable: true),
                    LastOperateUser = table.Column<string>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<short>(nullable: false),
                    IsSingle = table.Column<short>(nullable: false),
                    MallName = table.Column<string>(nullable: true),
                    MallCode = table.Column<string>(nullable: true),
                    OriginalVersionNo = table.Column<string>(nullable: true),
                    TargetVersionNo = table.Column<string>(nullable: true),
                    TargetVersionID = table.Column<byte[]>(nullable: false),
                    BusinessCount = table.Column<int>(nullable: false),
                    ReserveTime = table.Column<DateTime>(nullable: false),
                    ContactPerson = table.Column<string>(nullable: true),
                    ContactPhone = table.Column<string>(nullable: true),
                    UpgradeStatus = table.Column<int>(nullable: false),
                    ConfirmUpgradeTime = table.Column<DateTime>(nullable: true),
                    ApplyTime = table.Column<DateTime>(nullable: false),
                    HeartbeatTime = table.Column<DateTime>(nullable: false),
                    StartUpgradeTime = table.Column<DateTime>(nullable: true),
                    EndUpgradeTime = table.Column<DateTime>(nullable: true),
                    Summary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prc_UpgradeInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prc_UserInfo",
                columns: table => new
                {
                    ID = table.Column<byte[]>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateOperateUser = table.Column<string>(nullable: true),
                    LastOperateUser = table.Column<string>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<short>(nullable: false),
                    LoginName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    PassWord = table.Column<string>(nullable: true),
                    Rights = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prc_UserInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prc_VersionInfo",
                columns: table => new
                {
                    ID = table.Column<byte[]>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateOperateUser = table.Column<string>(nullable: true),
                    LastOperateUser = table.Column<string>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<short>(nullable: false),
                    IsPublish = table.Column<short>(nullable: false),
                    VersionNo = table.Column<string>(nullable: true),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Context = table.Column<byte[]>(nullable: true),
                    LastPublishTime = table.Column<DateTime>(nullable: true),
                    UpgradeBagName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prc_VersionInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prc_UpgradeInfoItem",
                columns: table => new
                {
                    ID = table.Column<byte[]>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateOperateUser = table.Column<string>(nullable: true),
                    LastOperateUser = table.Column<string>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<short>(nullable: false),
                    UpgradeInfoID = table.Column<byte[]>(nullable: true),
                    BusinessName = table.Column<string>(nullable: true),
                    BusinessType = table.Column<int>(nullable: false),
                    UpgradeBagStatus = table.Column<int>(nullable: false),
                    UpgradeStatus = table.Column<int>(nullable: false),
                    StartUpgradeTime = table.Column<DateTime>(nullable: true),
                    EndUpgradeTime = table.Column<DateTime>(nullable: true),
                    HeartbeatStatus = table.Column<int>(nullable: false),
                    BusinessNum = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prc_UpgradeInfoItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Prc_UpgradeInfoItem_Prc_UpgradeInfo_UpgradeInfoID",
                        column: x => x.UpgradeInfoID,
                        principalTable: "Prc_UpgradeInfo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prc_UpgradeMessage",
                columns: table => new
                {
                    ID = table.Column<byte[]>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateOperateUser = table.Column<string>(nullable: true),
                    LastOperateUser = table.Column<string>(nullable: true),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<short>(nullable: false),
                    UpgradeInfoItemID = table.Column<byte[]>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Result = table.Column<string>(nullable: true),
                    OccurTime = table.Column<DateTime>(nullable: false),
                    MessageType = table.Column<int>(nullable: false),
                    MessageFlag = table.Column<string>(nullable: true),
                    HandleStatus = table.Column<int>(nullable: false),
                    ExtendInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prc_UpgradeMessage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Prc_UpgradeMessage_Prc_UpgradeInfoItem_UpgradeInfoItemID",
                        column: x => x.UpgradeInfoItemID,
                        principalTable: "Prc_UpgradeInfoItem",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prc_UpgradeInfoItem_UpgradeInfoID",
                table: "Prc_UpgradeInfoItem",
                column: "UpgradeInfoID");

            migrationBuilder.CreateIndex(
                name: "IX_Prc_UpgradeMessage_UpgradeInfoItemID",
                table: "Prc_UpgradeMessage",
                column: "UpgradeInfoItemID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prc_ConfigSet");

            migrationBuilder.DropTable(
                name: "Prc_Holiday");

            migrationBuilder.DropTable(
                name: "Prc_UpgradeMessage");

            migrationBuilder.DropTable(
                name: "Prc_UserInfo");

            migrationBuilder.DropTable(
                name: "Prc_VersionInfo");

            migrationBuilder.DropTable(
                name: "Prc_UpgradeInfoItem");

            migrationBuilder.DropTable(
                name: "Prc_UpgradeInfo");
        }
    }
}
