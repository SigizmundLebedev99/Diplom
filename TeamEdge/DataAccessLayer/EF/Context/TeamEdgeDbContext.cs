using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using TeamEdge.DAL.Models;

namespace TeamEdge.DAL.Context
{
    public class TeamEdgeDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<Sprint> Sprints { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<_Repository> Repositories { get; set; }

        public virtual DbSet<Epick> Epicks { get; set; }

        public virtual DbSet<Feature> Features { get; set; }

        public virtual DbSet<UserStory> UserStories { get; set; }

        public virtual DbSet<_Task> Tasks { get; set; }

        public virtual DbSet<BranchLink> CodeLinks { get; set; }

        public virtual DbSet<WorkItemDescription> WorkItemDescriptions { get; set; }

        public virtual DbSet<UserProject> UserProjects { get; set; }

        public virtual DbSet<WorkItemTag> WorkItemTags { get; set; }

        public virtual DbSet<File> Files { get; set; }

        public virtual DbSet<TestCase> TestCases { get; set; }

        public virtual DbSet<WorkItemHistory> WorkItemHistories { get; set; }

        public virtual DbSet<Subscribe> Subscribes { get; set; }

        public virtual DbSet<Invite> Invites { get; set; }

        public virtual DbSet<WorkItemFile> WorkItemFiles { get; set; }

        public TeamEdgeDbContext(DbContextOptions<TeamEdgeDbContext> options) : base(options) { }

        public IQueryable<T> GetWorkItems<T>(Expression<Func<BaseWorkItem, bool>> filter, Expression<Func<BaseWorkItem, T>> selector)
        {
            return Tasks.Where(filter).Select(selector)
                .Concat(UserStories.Where(filter).Select(selector))
                .Concat(Epicks.Where(filter).Select(selector))
                .Concat(Features.Where(filter).Select(selector));
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WorkItemFile>().HasKey(e => new { e.FileId, e.WorkItemId });

            builder.Entity<UserProject>().HasKey(e => new { e.UserId, e.ProjectId });

            builder.Entity<WorkItemTag>().HasKey(e => new { e.Tag, e.WorkItemDescId });

            builder.Entity<Subscribe>().HasKey(e => new { e.SubscriberId, e.WorkItemId });

            builder.Entity<Invite>().HasOne(e => e.User).WithMany(u => u.Invites).HasForeignKey(e => e.ToUserId);

            foreach (var relationSheep in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationSheep.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
