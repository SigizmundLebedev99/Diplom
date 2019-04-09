﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TeamEdge;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;

namespace TeamEdge.Migrations
{
    [DbContext(typeof(TeamEdgeDbContext))]
    partial class TeamEdgeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models._File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatorId");

                    b.Property<DateTime>("DateOfCreation");

                    b.Property<string>("FileName")
                        .HasMaxLength(128);

                    b.Property<bool>("IsPicture");

                    b.Property<string>("Path");

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models._Task", b =>
                {
                    b.Property<int>("DescriptionId");

                    b.Property<int?>("AssignedToId");

                    b.Property<short?>("Duration");

                    b.Property<DateTime?>("EndDate");

                    b.Property<int?>("EpickId");

                    b.Property<int?>("GantPreviousId");

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.Property<int>("Number");

                    b.Property<int?>("ParentId");

                    b.Property<int?>("ParentSummaryTaskId");

                    b.Property<int?>("SprintId");

                    b.Property<DateTime?>("StartDate");

                    b.Property<byte>("Status");

                    b.Property<byte>("Type");

                    b.HasKey("DescriptionId");

                    b.HasIndex("AssignedToId");

                    b.HasIndex("EpickId");

                    b.HasIndex("GantPreviousId");

                    b.HasIndex("ParentId");

                    b.HasIndex("ParentSummaryTaskId");

                    b.HasIndex("SprintId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatorId");

                    b.Property<DateTime>("DateOfCreation");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<int>("WorkItemId");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("WorkItemId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.CommentFile", b =>
                {
                    b.Property<int>("FileId");

                    b.Property<int>("WorkItemId");

                    b.Property<int>("CommentId");

                    b.HasKey("FileId", "WorkItemId", "CommentId");

                    b.HasIndex("CommentId");

                    b.ToTable("CommentFiles");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.Epick", b =>
                {
                    b.Property<int>("DescriptionId");

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.Property<int>("Number");

                    b.Property<byte>("Status");

                    b.HasKey("DescriptionId");

                    b.ToTable("Epicks");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.Invite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatorId");

                    b.Property<DateTime>("DateOfCreation");

                    b.Property<bool>("IsAccepted");

                    b.Property<int>("ProjRole");

                    b.Property<int>("ProjectId");

                    b.Property<int>("RepoRole");

                    b.Property<int>("ToUserId");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ToUserId");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatorId");

                    b.Property<DateTime>("DateOfCreation");

                    b.Property<string>("Logo")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.Sprint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatorId");

                    b.Property<DateTime>("DateOfCreation");

                    b.Property<short?>("Duration");

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.Property<int>("ProjectId");

                    b.Property<DateTime?>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Sprints");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.Subscribe", b =>
                {
                    b.Property<int>("SubscriberId");

                    b.Property<int>("WorkItemId");

                    b.HasKey("SubscriberId", "WorkItemId");

                    b.HasIndex("WorkItemId");

                    b.ToTable("Subscribes");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.SubTask", b =>
                {
                    b.Property<int>("DescriptionId");

                    b.Property<int?>("AssignedToId");

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.Property<int>("Number");

                    b.Property<int?>("ParentId");

                    b.Property<float>("PersentOfWork");

                    b.Property<byte>("Status");

                    b.HasKey("DescriptionId");

                    b.HasIndex("AssignedToId");

                    b.HasIndex("ParentId");

                    b.ToTable("SubTasks");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.SummaryTask", b =>
                {
                    b.Property<int>("DescriptionId");

                    b.Property<short?>("Duration");

                    b.Property<DateTime?>("EndDate");

                    b.Property<int?>("GauntPreviousId");

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.Property<int>("Number");

                    b.Property<int?>("ParentId");

                    b.Property<int?>("ParentSummaryTaskId");

                    b.Property<DateTime?>("StartDate");

                    b.Property<byte>("Status");

                    b.HasKey("DescriptionId");

                    b.HasIndex("GauntPreviousId");

                    b.ToTable("SummaryTasks");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Avatar");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .HasMaxLength(64);

                    b.Property<string>("LastName")
                        .HasMaxLength(64);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("Patrinymic")
                        .HasMaxLength(64);

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.UserProject", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("ProjectId");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("ProjRole");

                    b.Property<int>("RepoRole");

                    b.HasKey("UserId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.ToTable("UserProjects");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.UserStory", b =>
                {
                    b.Property<int>("DescriptionId");

                    b.Property<string>("AcceptenceCriteria");

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.Property<int>("Number");

                    b.Property<int?>("ParentId");

                    b.Property<byte>("Priority");

                    b.Property<int?>("SprintId");

                    b.Property<byte>("Status");

                    b.HasKey("DescriptionId");

                    b.HasIndex("ParentId");

                    b.HasIndex("SprintId");

                    b.ToTable("UserStories");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.WorkItemDescription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatorId");

                    b.Property<DateTime>("DateOfCreation");

                    b.Property<string>("DescriptionText");

                    b.Property<DateTime?>("LastUpdate");

                    b.Property<int?>("LastUpdaterId");

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("LastUpdaterId");

                    b.HasIndex("ProjectId");

                    b.ToTable("WorkItemDescriptions");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.WorkItemFile", b =>
                {
                    b.Property<int>("FileId");

                    b.Property<int>("WorkItemId");

                    b.HasKey("FileId", "WorkItemId");

                    b.HasIndex("WorkItemId");

                    b.ToTable("WorkItemFiles");
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.WorkItemTag", b =>
                {
                    b.Property<string>("Tag")
                        .HasMaxLength(32);

                    b.Property<int>("WorkItemDescId");

                    b.HasKey("Tag", "WorkItemDescId");

                    b.HasIndex("WorkItemDescId");

                    b.ToTable("WorkItemTags");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models._File", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models._Task", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.User", "AssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedToId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.WorkItemDescription", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.Epick", "Epick")
                        .WithMany("Links")
                        .HasForeignKey("EpickId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.WorkItemDescription", "GantPrevious")
                        .WithMany()
                        .HasForeignKey("GantPreviousId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.UserStory", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.SummaryTask", "ParentSummaryTask")
                        .WithMany("Children")
                        .HasForeignKey("ParentSummaryTaskId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.Sprint")
                        .WithMany("Tasks")
                        .HasForeignKey("SprintId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.Comment", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.WorkItemDescription", "WorkItem")
                        .WithMany("Comments")
                        .HasForeignKey("WorkItemId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.CommentFile", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.Comment", "Comment")
                        .WithMany("Files")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.WorkItemFile", "WorkItemFile")
                        .WithMany("CommentFiles")
                        .HasForeignKey("FileId", "WorkItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.Epick", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.WorkItemDescription", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.Invite", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.User", "User")
                        .WithMany("Invites")
                        .HasForeignKey("ToUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.Project", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.Sprint", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.Subscribe", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.User", "Subscriber")
                        .WithMany("Subscribes")
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.WorkItemDescription", "WorkItem")
                        .WithMany("Subscribers")
                        .HasForeignKey("WorkItemId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.SubTask", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.User", "AssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedToId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.WorkItemDescription", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models._Task", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.SummaryTask", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.WorkItemDescription", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.WorkItemDescription", "GauntPrevious")
                        .WithMany()
                        .HasForeignKey("GauntPreviousId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.UserProject", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.Project", "Project")
                        .WithMany("Users")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.User", "User")
                        .WithMany("UserProjects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.UserStory", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.WorkItemDescription", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.Epick", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.Sprint", "Sprint")
                        .WithMany("UserStories")
                        .HasForeignKey("SprintId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.WorkItemDescription", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.User", "LastUpdater")
                        .WithMany()
                        .HasForeignKey("LastUpdaterId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TeamEdge.DAL.Models.Project", "Project")
                        .WithMany("WorkItemDescriptions")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.WorkItemFile", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models._File", "File")
                        .WithMany("WorkItemFiles")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TeamEdge.DAL.Models.WorkItemDescription", "WorkItem")
                        .WithMany("Files")
                        .HasForeignKey("WorkItemId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TeamEdge.DAL.Models.WorkItemTag", b =>
                {
                    b.HasOne("TeamEdge.DAL.Models.WorkItemDescription", "WorkItem")
                        .WithMany("Tags")
                        .HasForeignKey("WorkItemDescId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
