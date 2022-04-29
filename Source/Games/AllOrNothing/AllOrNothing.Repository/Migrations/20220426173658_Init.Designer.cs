﻿// <auto-generated />
using System;
using AllOrNothing.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AllOrNothing.Repository.Migrations
{
    [DbContext(typeof(AllOrNothingDbContext))]
    [Migration("20220426173658_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.16");

            modelBuilder.Entity("AllOrNothing.Data.Competence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Competences");
                });

            modelBuilder.Entity("AllOrNothing.Data.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Institute")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NickName")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TeamId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("AllOrNothing.Data.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Answer")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Resource")
                        .HasColumnType("BLOB");

                    b.Property<int>("ResourceType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("TopicId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("AllOrNothing.Data.QuestionSerie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("QuestionSeries");
                });

            modelBuilder.Entity("AllOrNothing.Data.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("AllOrNothing.Data.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("QuestionSerieId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("QuestionSerieId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("CompetenceTopic", b =>
                {
                    b.Property<int>("CompetencesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TopicsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CompetencesId", "TopicsId");

                    b.HasIndex("TopicsId");

                    b.ToTable("CompetenceTopic");
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

            modelBuilder.Entity("CompetenceTopic", b =>
                {
                    b.HasOne("AllOrNothing.Data.Competence", null)
                        .WithMany()
                        .HasForeignKey("CompetencesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AllOrNothing.Data.Topic", null)
                        .WithMany()
                        .HasForeignKey("TopicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}