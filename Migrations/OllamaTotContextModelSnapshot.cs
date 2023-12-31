﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OllamaTot.Models;

#nullable disable

namespace OllamaToT.Migrations
{
    [DbContext(typeof(OllamaTotContext))]
    partial class OllamaTotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OllamaTot.Models.Prompt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Prompts");
                });

            modelBuilder.Entity("OllamaTot.Models.TotTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EvaluatePromptId")
                        .HasColumnType("integer");

                    b.Property<int>("ProposePromptId")
                        .HasColumnType("integer");

                    b.Property<string>("Response")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EvaluatePromptId");

                    b.HasIndex("ProposePromptId");

                    b.ToTable("TotTasks");
                });

            modelBuilder.Entity("OllamaTot.Models.TotTask", b =>
                {
                    b.HasOne("OllamaTot.Models.Prompt", "EvaluatePrompt")
                        .WithMany("EvaluateTasks")
                        .HasForeignKey("EvaluatePromptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OllamaTot.Models.Prompt", "ProposePrompt")
                        .WithMany("ProposeTasks")
                        .HasForeignKey("ProposePromptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EvaluatePrompt");

                    b.Navigation("ProposePrompt");
                });

            modelBuilder.Entity("OllamaTot.Models.Prompt", b =>
                {
                    b.Navigation("EvaluateTasks");

                    b.Navigation("ProposeTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
