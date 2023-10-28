﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SSU.DM.DataAccessLayer.Core;

#nullable disable

namespace SSU.DM.DataAccessLayer.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20231016190816_add_dative")]
    partial class add_dative
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SSU.DM.DataAccessLayer.DbEntities.ApplicationForm", b =>
                {
                    b.Property<Guid>("ApplicationFormId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("DateTimeCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("FacultyId")
                        .HasColumnType("integer");

                    b.Property<string>("FileKey")
                        .HasColumnType("text");

                    b.HasKey("ApplicationFormId");

                    b.HasIndex("FacultyId");

                    b.HasIndex("FileKey");

                    b.ToTable("ApplicationForms");
                });

            modelBuilder.Entity("SSU.DM.DataAccessLayer.DbEntities.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("NameDat")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("SSU.DM.DataAccessLayer.DbEntities.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("ApplicationFormId")
                        .HasColumnType("uuid");

                    b.Property<int>("BudgetCount")
                        .HasColumnType("integer");

                    b.Property<int>("CommercialCount")
                        .HasColumnType("integer");

                    b.Property<string>("Direction")
                        .HasColumnType("text");

                    b.Property<string>("GroupForm")
                        .HasColumnType("text");

                    b.Property<string>("GroupNumber")
                        .HasColumnType("text");

                    b.Property<int>("IndependentWorkHours")
                        .HasColumnType("integer");

                    b.Property<int>("LaboratoryHours")
                        .HasColumnType("integer");

                    b.Property<int>("LectureHours")
                        .HasColumnType("integer");

                    b.Property<string>("NameDiscipline")
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<int>("PracticalHours")
                        .HasColumnType("integer");

                    b.Property<int>("Reporting")
                        .HasColumnType("integer");

                    b.Property<int>("Semester")
                        .HasColumnType("integer");

                    b.Property<int>("TotalHours")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationFormId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("SSU.DM.DataAccessLayer.DbEntities.SavedFile", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<byte[]>("Bytes")
                        .HasColumnType("bytea");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.HasKey("Key");

                    b.ToTable("SavedFiles");
                });

            modelBuilder.Entity("SSU.DM.DataAccessLayer.DbEntities.ApplicationForm", b =>
                {
                    b.HasOne("SSU.DM.DataAccessLayer.DbEntities.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId");

                    b.HasOne("SSU.DM.DataAccessLayer.DbEntities.SavedFile", "File")
                        .WithMany()
                        .HasForeignKey("FileKey");

                    b.Navigation("Faculty");

                    b.Navigation("File");
                });

            modelBuilder.Entity("SSU.DM.DataAccessLayer.DbEntities.Request", b =>
                {
                    b.HasOne("SSU.DM.DataAccessLayer.DbEntities.ApplicationForm", "ApplicationForm")
                        .WithMany("Requests")
                        .HasForeignKey("ApplicationFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationForm");
                });

            modelBuilder.Entity("SSU.DM.DataAccessLayer.DbEntities.ApplicationForm", b =>
                {
                    b.Navigation("Requests");
                });
#pragma warning restore 612, 618
        }
    }
}
