using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Security.Claims;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Cuillere.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Notez qu'authenticationType doit correspondre à l'élément défini dans CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Ajouter les revendications personnalisées de l’utilisateur ici
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public ApplicationDbContext(string dbName) : base(GetConnectionString(dbName))
        //{
        //}

        public ApplicationDbContext() : base("mysqlCon")
        {
        }

        //public static string GetConnectionString(string dbName)
        //{
        //    // Server=localhost;Database={0};Uid=username;Pwd=password
        //    var connString =
        //        ConfigurationManager.ConnectionStrings["mysqlCon"].ConnectionString.ToString();

        //    return String.Format(connString, dbName);
        //}

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public class MigrationsContextFactory : IDbContextFactory<ApplicationDbContext>
        {
            public ApplicationDbContext Create()
            {
                return new ApplicationDbContext();
            }
        }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Rayon> Rayons { get; set; }
        public DbSet<Recette> Recettes { get; set; }
        public DbSet<RecetteDetail> RecetteDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CartItem> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Saison> Saisons { get; set; }
    } 
}