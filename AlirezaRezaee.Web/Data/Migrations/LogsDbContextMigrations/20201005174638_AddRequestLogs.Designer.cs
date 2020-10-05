﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rezaee.Alireza.Web.Data;

namespace Rezaee.Alireza.Web.Data.Migrations.LogsDbContextMigrations
{
    [DbContext(typeof(LogsDbContext))]
    [Migration("20201005174638_AddRequestLogs")]
    partial class AddRequestLogs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Rezaee.Alireza.Web.Models.Requestlogs", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FilesPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Host")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Method")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Protocol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QueryString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Referrer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestBodyFilePathPostfix")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestHeadersFilePathPostfix")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseBodyFilePathPostfix")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ResponseContentLength")
                        .HasColumnType("bigint");

                    b.Property<string>("ResponseHeadersFilePathPostfix")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Scheme")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusCode")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Requestlogs");
                });
#pragma warning restore 612, 618
        }
    }
}
