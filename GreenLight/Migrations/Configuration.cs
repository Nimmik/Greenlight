using GreenLight.Models;

namespace GreenLight.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GreenLight.Models.Context.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GreenLight.Models.Context.ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            if (!(context.Users.Any(u => u.UserName == "admin@t.t")))
            {
                var userToInsert = new ApplicationUser { UserName = "admin@t.t", Email = "admin@t.t" };
                userManager.Create(userToInsert, "Password@123");
            }

            context.Posts.AddOrUpdate(
              new Post()
              {
                  Id = 1,
                  Description = "SOME TEST",
                  PostedById = userManager.FindByEmail("admin@t.t").Id,
                  PostedBy = userManager.FindByEmail("admin@t.t"),
                  Views = 2,
                  Result = null,
                  Title = "A GOOD TITLE"
              }
            );

            context.Posts.AddOrUpdate(
              new Post()
              {
                  Id = 2,
                  Description = "This is a post",
                  PostedById = userManager.FindByEmail("admin@t.t").Id,
                  PostedBy = userManager.FindByEmail("admin@t.t"),
                  Views = 25,
                  Result = false,
                  Title = "Hello world"
              }
            );

            context.Posts.AddOrUpdate(
              new Post()
              {
                  Id = 3,
                  Description = "This was a post",
                  PostedById = userManager.FindByEmail("admin@t.t").Id,
                  PostedBy = userManager.FindByEmail("admin@t.t"),
                  Views = 1212,
                  Result = true,
                  Title = "My light is green"
              }
            );

            context.Comments.AddOrUpdate(
                new Comment()
                {
                    Id = 1,
                    CreatedById = userManager.FindByEmail("admin@t.t").Id,
                    CreatedBy = userManager.FindByEmail("admin@t.t"),
                    Writing = "Something Written",
                    PostId = 1,
                    Post = context.Posts.Find(1),
                    CreatedOn = DateTime.Now,
                    Likes = 2
                }
            );

            context.Votes.AddOrUpdate(
                new Vote(1)
                {
                    Id = 1,
                    VoterId = userManager.FindByEmail("admin@t.t").Id,
                    PostId = 1,
                    OnOff = true
                }
            );
        }
    }
}
