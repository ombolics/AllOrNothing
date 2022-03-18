﻿// <auto-generated />
using System;
using AllOrNothing.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AllOrNothing.Repository.Migrations
{
    [DbContext(typeof(AllOrNothingDbContext))]
    partial class AllOrNothingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "6.0.0-preview.5.21301.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AllOrNothing.Data.Competence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TopicId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Competences");
                });

            modelBuilder.Entity("AllOrNothing.Data.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Institue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NickName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("AllOrNothing.Data.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Resource")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("ResourceType")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TopicId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("AllOrNothing.Data.QuestionSerie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("QuestionSeries");
                });

            modelBuilder.Entity("AllOrNothing.Data.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("AllOrNothing.Data.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("QuestionSerieId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("QuestionSerieId");

                    b.ToTable("Topic");
                });

            modelBuilder.Entity("AllOrNothing.Data.Competence", b =>
                {
                    b.HasOne("AllOrNothing.Data.Topic", null)
                        .WithMany("Competences")
                        .HasForeignKey("TopicId");
                });

            modelBuilder.Entity("AllOrNothing.Data.Player", b =>
                {
                    b.HasOne("AllOrNothing.Data.Team", null)
                        .WithMany("Players")
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("AllOrNothing.Data.Question", b =>
                {
                    b.HasOne("AllOrNothing.Data.Topic", null)
                        .WithMany("Questions")
                        .HasForeignKey("TopicId");
                });

            modelBuilder.Entity("AllOrNothing.Data.Topic", b =>
                {
                    b.HasOne("AllOrNothing.Data.Player", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("AllOrNothing.Data.QuestionSerie", null)
                        .WithMany("Topics")
                        .HasForeignKey("QuestionSerieId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("AllOrNothing.Data.QuestionSerie", b =>
                {
                    b.Navigation("Topics");
                });

            modelBuilder.Entity("AllOrNothing.Data.Team", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("AllOrNothing.Data.Topic", b =>
                {
                    b.Navigation("Competences");

                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
