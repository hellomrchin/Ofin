﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OFIN.Models.Merchant;

namespace OFIN.Migrations.Supplier
{
    [DbContext(typeof(SupplierContext))]
    partial class SupplierContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OFIN.Models.Merchant.Suppliers", b =>
                {
                    b.Property<string>("fSupplierCode");

                    b.Property<string>("fRemark");

                    b.Property<string>("fSupplierEmail");

                    b.Property<string>("fSupplierName");

                    b.Property<string>("fSupplierPhone");

                    b.HasKey("fSupplierCode");

                    b.ToTable("tSupplier");
                });
#pragma warning restore 612, 618
        }
    }
}
