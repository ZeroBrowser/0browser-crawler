﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZeroBrowser.Crawler.Core;

namespace ZeroBrowser.Crawler.Core.Migrations
{
    [DbContext(typeof(CrawlerDBContext))]
    partial class CrawlerDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("ZeroBrowser.Crawler.Core.CrawlerDBContext+CrawledRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("HashedUrl")
                        .HasColumnType("TEXT");

                    b.Property<int>("HealthStatus")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("HttpStatusCode")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Inserted")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("HashedUrl");

                    b.ToTable("CrawledRecords");
                });

            modelBuilder.Entity("ZeroBrowser.Crawler.Core.CrawlerDBContext+CrawledRecordRelation", b =>
                {
                    b.Property<Guid>("ParentId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ChildId")
                        .HasColumnType("TEXT");

                    b.HasKey("ParentId", "ChildId");

                    b.HasIndex("ChildId");

                    b.ToTable("CrawledRecordRelations");
                });

            modelBuilder.Entity("ZeroBrowser.Crawler.Core.CrawlerDBContext+CrawledRecordRelation", b =>
                {
                    b.HasOne("ZeroBrowser.Crawler.Core.CrawlerDBContext+CrawledRecord", "Child")
                        .WithMany("ParentCrawledRecord")
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ZeroBrowser.Crawler.Core.CrawlerDBContext+CrawledRecord", "Parent")
                        .WithMany("CrawledRecords")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Child");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("ZeroBrowser.Crawler.Core.CrawlerDBContext+CrawledRecord", b =>
                {
                    b.Navigation("CrawledRecords");

                    b.Navigation("ParentCrawledRecord");
                });
#pragma warning restore 612, 618
        }
    }
}