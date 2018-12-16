using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TeamEdge.DAL.Models;

namespace TeamEdge.DAL.Context
{
    public class TeamEdgeDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Project> Projects { get; set; }

        public DbSet<Sprint> Sprints { get; set; }

        public DbSet<Comment> Comments { get; set; }
        
        public DbSet<Repository> Repositories { get; set; }

        public DbSet<Epick> Epicks { get; set; }

        public DbSet<Feature> Features { get; set; }

        public DbSet<UserStory> UserStories { get; set; }

        public DbSet<_Task> Tasks { get; set; }

        public DbSet<BranchLink> CodeLinks { get; set; }

        public DbSet<WorkItemDescription> WorkItemDescriptions { get; set; }

        public DbSet<UserProject> UserProjects { get; set; }

        public DbSet<WorkItemTag> WorkItemTags { get; set; }

        public DbSet<WorkItemFile> WorkItemFiles { get; set; }

        public DbSet<TestCase> TestCases { get; set; }

        public DbSet<WorkItemHistory> WorkItemHistories { get; set; }

        public DbSet<Subscribe> Subscribes { get; set; }

        public TeamEdgeDbContext(DbContextOptions<TeamEdgeDbContext> options) : base(options) { }

        public DbSet<Invite> Invites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserProject>().HasKey(e => new { e.UserId, e.ProjectId });

            builder.Entity<WorkItemTag>().HasKey(e => new { e.Tag, e.WorkItemDescId });

            builder.Entity<Subscribe>().HasKey(e => new { e.SubscriberId, e.WorkItemId });

            foreach (var relationSheep in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationSheep.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
