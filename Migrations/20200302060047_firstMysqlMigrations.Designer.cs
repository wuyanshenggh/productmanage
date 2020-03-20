﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductMange.Model;

namespace ProductManageNew.Migrations
{
    [DbContext(typeof(ProductManageDbContext))]
    [Migration("20200302060047_firstMysqlMigrations")]
    partial class firstMysqlMigrations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("ProductMange.Model.Prc_ConfigSet", b =>
                {
                    b.Property<byte[]>("ID")
                        .ValueGeneratedOnAdd()
                        .HasConversion(new ValueConverter<byte[], byte[]>(v => default(byte[]), v => default(byte[]), new ConverterMappingHints(size: 16)));

                    b.Property<int>("Code");

                    b.Property<string>("CreateOperateUser");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("LastOperateUser");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Value");

                    b.HasKey("ID");

                    b.ToTable("Prc_ConfigSet");
                });

            modelBuilder.Entity("ProductMange.Model.Prc_Holiday", b =>
                {
                    b.Property<byte[]>("ID")
                        .ValueGeneratedOnAdd()
                        .HasConversion(new ValueConverter<byte[], byte[]>(v => default(byte[]), v => default(byte[]), new ConverterMappingHints(size: 16)));

                    b.Property<string>("CreateOperateUser");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Data");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("LastOperateUser");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<int>("Year");

                    b.HasKey("ID");

                    b.ToTable("Prc_Holiday");
                });

            modelBuilder.Entity("ProductMange.Model.Prc_UpgradeInfo", b =>
                {
                    b.Property<byte[]>("ID")
                        .ValueGeneratedOnAdd()
                        .HasConversion(new ValueConverter<byte[], byte[]>(v => default(byte[]), v => default(byte[]), new ConverterMappingHints(size: 16)));

                    b.Property<DateTime>("ApplyTime");

                    b.Property<int>("BusinessCount");

                    b.Property<DateTime?>("ConfirmUpgradeTime");

                    b.Property<string>("ContactPerson");

                    b.Property<string>("ContactPhone");

                    b.Property<string>("CreateOperateUser");

                    b.Property<DateTime>("CreateTime");

                    b.Property<DateTime?>("EndUpgradeTime");

                    b.Property<DateTime>("HeartbeatTime");

                    b.Property<bool>("IsDelete");

                    b.Property<bool>("IsSingle");

                    b.Property<string>("LastOperateUser");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("MallCode");

                    b.Property<string>("MallName");

                    b.Property<string>("OriginalVersionNo");

                    b.Property<DateTime>("ReserveTime");

                    b.Property<DateTime?>("StartUpgradeTime");

                    b.Property<string>("Summary");

                    b.Property<byte[]>("TargetVersionID")
                        .IsRequired()
                        .HasConversion(new ValueConverter<byte[], byte[]>(v => default(byte[]), v => default(byte[]), new ConverterMappingHints(size: 16)));

                    b.Property<string>("TargetVersionNo");

                    b.Property<int>("UpgradeStatus");

                    b.HasKey("ID");

                    b.ToTable("Prc_UpgradeInfo");
                });

            modelBuilder.Entity("ProductMange.Model.Prc_UpgradeInfoItem", b =>
                {
                    b.Property<byte[]>("ID")
                        .ValueGeneratedOnAdd()
                        .HasConversion(new ValueConverter<byte[], byte[]>(v => default(byte[]), v => default(byte[]), new ConverterMappingHints(size: 16)));

                    b.Property<string>("BusinessName");

                    b.Property<string>("BusinessNum");

                    b.Property<int>("BusinessType");

                    b.Property<string>("CreateOperateUser");

                    b.Property<DateTime>("CreateTime");

                    b.Property<DateTime?>("EndUpgradeTime");

                    b.Property<int>("HeartbeatStatus");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("LastOperateUser");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<DateTime?>("StartUpgradeTime");

                    b.Property<int>("UpgradeBagStatus");

                    b.Property<byte[]>("UpgradeInfoID")
                        .HasConversion(new ValueConverter<byte[], byte[]>(v => default(byte[]), v => default(byte[]), new ConverterMappingHints(size: 16)));

                    b.Property<int>("UpgradeStatus");

                    b.HasKey("ID");

                    b.HasIndex("UpgradeInfoID");

                    b.ToTable("Prc_UpgradeInfoItem");
                });

            modelBuilder.Entity("ProductMange.Model.Prc_UpgradeMessage", b =>
                {
                    b.Property<byte[]>("ID")
                        .ValueGeneratedOnAdd()
                        .HasConversion(new ValueConverter<byte[], byte[]>(v => default(byte[]), v => default(byte[]), new ConverterMappingHints(size: 16)));

                    b.Property<string>("Content");

                    b.Property<string>("CreateOperateUser");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("ExtendInfo");

                    b.Property<int>("HandleStatus");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("LastOperateUser");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("MessageFlag");

                    b.Property<int>("MessageType");

                    b.Property<DateTime>("OccurTime");

                    b.Property<string>("Result");

                    b.Property<byte[]>("UpgradeInfoItemID")
                        .HasConversion(new ValueConverter<byte[], byte[]>(v => default(byte[]), v => default(byte[]), new ConverterMappingHints(size: 16)));

                    b.HasKey("ID");

                    b.HasIndex("UpgradeInfoItemID");

                    b.ToTable("Prc_UpgradeMessage");
                });

            modelBuilder.Entity("ProductMange.Model.Prc_UserInfo", b =>
                {
                    b.Property<byte[]>("ID")
                        .ValueGeneratedOnAdd()
                        .HasConversion(new ValueConverter<byte[], byte[]>(v => default(byte[]), v => default(byte[]), new ConverterMappingHints(size: 16)));

                    b.Property<string>("CreateOperateUser");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("LastOperateUser");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("LoginName");

                    b.Property<string>("PassWord");

                    b.Property<string>("Rights")
                        .HasMaxLength(500);

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.ToTable("Prc_UserInfo");
                });

            modelBuilder.Entity("ProductMange.Model.Prc_VersionInfo", b =>
                {
                    b.Property<byte[]>("ID")
                        .ValueGeneratedOnAdd()
                        .HasConversion(new ValueConverter<byte[], byte[]>(v => default(byte[]), v => default(byte[]), new ConverterMappingHints(size: 16)));

                    b.Property<byte[]>("Context");

                    b.Property<string>("CreateOperateUser");

                    b.Property<DateTime>("CreateTime");

                    b.Property<bool>("IsDelete");

                    b.Property<bool>("IsPublish");

                    b.Property<string>("LastOperateUser");

                    b.Property<DateTime?>("LastPublishTime");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<DateTime>("PublishDate");

                    b.Property<string>("UpgradeBagName");

                    b.Property<string>("VersionNo");

                    b.HasKey("ID");

                    b.ToTable("Prc_VersionInfo");
                });

            modelBuilder.Entity("ProductMange.Model.Prc_UpgradeInfoItem", b =>
                {
                    b.HasOne("ProductMange.Model.Prc_UpgradeInfo", "UpgradeInfo")
                        .WithMany()
                        .HasForeignKey("UpgradeInfoID");
                });

            modelBuilder.Entity("ProductMange.Model.Prc_UpgradeMessage", b =>
                {
                    b.HasOne("ProductMange.Model.Prc_UpgradeInfoItem", "UpgradeInfoItem")
                        .WithMany()
                        .HasForeignKey("UpgradeInfoItemID");
                });
#pragma warning restore 612, 618
        }
    }
}
