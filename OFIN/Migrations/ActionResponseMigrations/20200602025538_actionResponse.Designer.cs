﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OFIN.Models;

namespace OFIN.Migrations.ActionResponseMigrations
{
    [DbContext(typeof(ActionResponse))]
    [Migration("20200602025538_actionResponse")]
    partial class actionResponse
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OFIN.Models.ActionResponses", b =>
                {
                    b.Property<string>("fErrorCode")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("fErrorDesc");

                    b.Property<string>("fErrorMsg");

                    b.HasKey("fErrorCode");

                    b.ToTable("tActionResponses");
                });
#pragma warning restore 612, 618
        }
    }
}
