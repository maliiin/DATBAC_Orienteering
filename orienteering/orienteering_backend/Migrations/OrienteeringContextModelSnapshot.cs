﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using orienteering_backend.Infrastructure.Data;

#nullable disable

namespace orienteering_backend.Migrations
{
    [DbContext(typeof(OrienteeringContext))]
    partial class OrienteeringContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("orienteering_backend.Core.Domain.Track.Checkpoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("GameId")
                        .HasColumnType("int");

                    b.Property<int?>("QuizId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("TrackId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("TrackId");

                    b.ToTable("Checkpoints");
                });

            modelBuilder.Entity("orienteering_backend.Core.Domain.Track.Track", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tracks");
                });

            modelBuilder.Entity("orienteering_backend.Core.Domain.Track.Checkpoint", b =>
                {
                    b.HasOne("orienteering_backend.Core.Domain.Track.Track", null)
                        .WithMany("CheckpointList")
                        .HasForeignKey("TrackId");
                });

            modelBuilder.Entity("orienteering_backend.Core.Domain.Track.Track", b =>
                {
                    b.Navigation("CheckpointList");
                });
#pragma warning restore 612, 618
        }
    }
}
