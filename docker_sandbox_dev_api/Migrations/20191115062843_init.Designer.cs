﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using docker_sandbox_dev_api.Dal;

namespace docker_sandbox_dev_api.Migrations
{
    [DbContext(typeof(SandboxDbContext))]
    [Migration("20191115062843_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("docker_sandbox_dev_api.Dal.Model.Container", b =>
                {
                    b.Property<Guid>("ContainerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ContainerId");

                    b.HasIndex("UserId");

                    b.ToTable("Containers");
                });

            modelBuilder.Entity("docker_sandbox_dev_api.Dal.Model.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("docker_sandbox_dev_api.Dal.Model.Container", b =>
                {
                    b.HasOne("docker_sandbox_dev_api.Dal.Model.User", "User")
                        .WithMany("Containers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
