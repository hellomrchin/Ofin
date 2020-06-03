﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OFIN.Models.Account;

namespace OFIN.Migrations.Admin
{
    [DbContext(typeof(AdminContext))]
    [Migration("20200602053336_Admin")]
    partial class Admin
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OFIN.Models.Account.Admin", b =>
                {
                    b.Property<string>("fUsername")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("fAccessToken");

                    b.Property<string>("fIsVerified");

                    b.Property<DateTime>("fLastLogin");

                    b.Property<string>("fPassword");

                    b.Property<DateTime>("fRegTime");

                    b.HasKey("fUsername");

                    b.ToTable("tAdmin");
                });
#pragma warning restore 612, 618
        }
    }
}
