using CinemaManagementSystem.ENTITY;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagementSystem.WEBUI.Identity
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        //public DbSet<User> User { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)

            { 

            }
 
    }
 }
