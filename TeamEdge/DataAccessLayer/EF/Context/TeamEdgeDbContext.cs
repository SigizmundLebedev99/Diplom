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

        public virtual DbSet<Epick> Epicks { get; set; }

        public virtual DbSet<UserStory> UserStories { get; set; }

        public virtual DbSet<_Task> Tasks { get; set; }

        public virtual DbSet<WorkItemDescription> WorkItemDescriptions { get; set; }

        public virtual DbSet<UserProject> UserProjects { get; set; }

        public virtual DbSet<WorkItemTag> WorkItemTags { get; set; }

        public virtual DbSet<_File> Files { get; set; }

        public virtual DbSet<Subscribe> Subscribes { get; set; }

        public virtual DbSet<Invite> Invites { get; set; }

        public virtual DbSet<WorkItemFile> WorkItemFiles { get; set; }

        public virtual DbSet<SubTask> SubTasks { get; set; }

        public virtual DbSet<CommentFile> CommentFiles { get; set; }

        public virtual DbSet<SummaryTask> SummaryTasks { get; set; }

        public TeamEdgeDbContext(DbContextOptions<TeamEdgeDbContext> options) : base(options) { }

        public IQueryable<T> GetWorkItems<T>(Expression<Func<IBaseWorkItem, bool>> filter, Expression<Func<IBaseWorkItem, T>> selector)
        {
            return Tasks.Where(filter).Select(selector)
                .Concat(UserStories.Where(filter).Select(selector))
                .Concat(Epicks.Where(filter).Select(selector))
                .Concat(SummaryTasks.Where(filter).Select(selector));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserProject>().HasKey(e => new { e.UserId, e.ProjectId });

            builder.Entity<WorkItemTag>().HasKey(e => new { e.Tag, e.WorkItemDescId });

            builder.Entity<Subscribe>().HasKey(e => new { e.SubscriberId, e.WorkItemId });

            builder.Entity<Invite>().HasOne(e => e.User).WithMany(u => u.Invites).HasForeignKey(e => e.ToUserId);

            foreach (var relationSheep in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationSheep.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<WorkItemFile>(ent =>
            {
                ent.HasKey(e => new { e.FileId, e.WorkItemId });
                ent.HasOne(e => e.File).WithMany(e => e.WorkItemFiles).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<CommentFile>(ent =>
            {
                ent.HasKey(e => new { e.FileId, e.WorkItemId, e.CommentId});
                ent
                .HasOne(e => e.WorkItemFile)
                .WithMany(e => e.CommentFiles)
                .HasForeignKey(e=>new { e.FileId, e.WorkItemId})
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
