﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Data;

namespace WebApi.Migrations
{
    [DbContext(typeof(MedStationContext))]
    [Migration("20190305140316_Patient_Implementation")]
    partial class Patient_Implementation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApi.Models.CheckIn", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AppointedTo");

                    b.Property<DateTime>("Arrival");

                    b.Property<bool>("IsAppointment");

                    b.Property<bool>("IsDone");

                    b.Property<long?>("PatientId");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("CheckIns");
                });

            modelBuilder.Entity("WebApi.Models.HistoryEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<long?>("PatientId");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("HistoryEntry");
                });

            modelBuilder.Entity("WebApi.Models.Medicine", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("HistoryEntryId");

                    b.Property<string>("Manufacturer");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("HistoryEntryId");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("WebApi.Models.Patient", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.Property<string>("SocialSecurityId");

                    b.HasKey("Id");

                    b.HasIndex("SocialSecurityId")
                        .IsUnique()
                        .HasFilter("[SocialSecurityId] IS NOT NULL");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("WebApi.Models.CheckIn", b =>
                {
                    b.HasOne("WebApi.Models.Patient", "Patient")
                        .WithMany("CheckIns")
                        .HasForeignKey("PatientId");
                });

            modelBuilder.Entity("WebApi.Models.HistoryEntry", b =>
                {
                    b.HasOne("WebApi.Models.Patient")
                        .WithMany("MedicalHistory")
                        .HasForeignKey("PatientId");
                });

            modelBuilder.Entity("WebApi.Models.Medicine", b =>
                {
                    b.HasOne("WebApi.Models.HistoryEntry")
                        .WithMany("Prescription")
                        .HasForeignKey("HistoryEntryId");
                });
#pragma warning restore 612, 618
        }
    }
}