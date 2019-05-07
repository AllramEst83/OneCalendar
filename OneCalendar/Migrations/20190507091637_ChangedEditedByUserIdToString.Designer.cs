﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OneCalendar.Context;

namespace OneCalendar.Migrations
{
    [DbContext(typeof(CalenderContext))]
    [Migration("20190507091637_ChangedEditedByUserIdToString")]
    partial class ChangedEditedByUserIdToString
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OneCalendar.Models.CalenderGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IdsCollection");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("CalenderGroups");
                });

            modelBuilder.Entity("OneCalendar.Models.CalenderTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CalenderGroupId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("TaskDescription");

                    b.Property<string>("TaskName");

                    b.HasKey("Id");

                    b.HasIndex("CalenderGroupId");

                    b.ToTable("CalenderTasks");
                });

            modelBuilder.Entity("OneCalendar.Models.EditedByUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CalenderTaskId");

                    b.Property<DateTime>("DateOfEdit");

                    b.Property<string>("EditedByUserId");

                    b.HasKey("Id");

                    b.HasIndex("CalenderTaskId");

                    b.ToTable("EditedByUser");
                });

            modelBuilder.Entity("OneCalendar.Models.CalenderTask", b =>
                {
                    b.HasOne("OneCalendar.Models.CalenderGroup")
                        .WithMany("CalenderTasks")
                        .HasForeignKey("CalenderGroupId");
                });

            modelBuilder.Entity("OneCalendar.Models.EditedByUser", b =>
                {
                    b.HasOne("OneCalendar.Models.CalenderTask")
                        .WithMany("Edited")
                        .HasForeignKey("CalenderTaskId");
                });
#pragma warning restore 612, 618
        }
    }
}
