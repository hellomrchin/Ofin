﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OFIN.Models.Account;

namespace OFIN.Migrations.UserDetail
{
    [DbContext(typeof(UserDetailContext))]
    [Migration("20200602022344_UserDetail02")]
    partial class UserDetail02
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OFIN.Models.Account.UserDetail", b =>
                {
                    b.Property<string>("fUsername")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("fFirstName");

                    b.Property<string>("fGender");

                    b.Property<string>("fIcNumber");

                    b.Property<string>("fLastName");

                    b.HasKey("fUsername");

                    b.ToTable("tUserDetail");
                });
#pragma warning restore 612, 618
        }
    }
}
