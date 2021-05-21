﻿// <auto-generated />
using ApplicationCore.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApplicationCore.Migrations.RealTime
{
    [DbContext(typeof(RealTimeContext))]
    [Migration("20210508052813_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ApplicationCore.Models.Data", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Date")
                        .HasColumnType("int");

                    b.Property<string>("Indicator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuoteId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Time")
                        .HasColumnType("int");

                    b.Property<string>("Val")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuoteId");

                    b.ToTable("Data");
                });

            modelBuilder.Entity("ApplicationCore.Models.Quote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Date")
                        .HasColumnType("int");

                    b.Property<int>("High")
                        .HasColumnType("int");

                    b.Property<int>("Low")
                        .HasColumnType("int");

                    b.Property<int>("Open")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Time")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Quotes");
                });

            modelBuilder.Entity("ApplicationCore.Models.Data", b =>
                {
                    b.HasOne("ApplicationCore.Models.Quote", "Quote")
                        .WithMany("DataList")
                        .HasForeignKey("QuoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quote");
                });

            modelBuilder.Entity("ApplicationCore.Models.Quote", b =>
                {
                    b.Navigation("DataList");
                });
#pragma warning restore 612, 618
        }
    }
}
