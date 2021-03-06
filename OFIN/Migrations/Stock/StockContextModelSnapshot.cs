﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OFIN.Models.Merchant;

namespace OFIN.Migrations.Stock
{
    [DbContext(typeof(StockContext))]
    partial class StockContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OFIN.Models.Merchant.Stocks", b =>
                {
                    b.Property<string>("ItemCode");

                    b.Property<int>("ItemStock");

                    b.Property<DateTime>("fLastUpdate");

                    b.HasKey("ItemCode");

                    b.ToTable("tStock");
                });
#pragma warning restore 612, 618
        }
    }
}
