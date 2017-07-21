using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Cuillere.Models;

namespace Cuillere
{
    
        
        //protected override void Seed(ApplicationDbContext context)
        //{
        //    IList<Category> defaultCategories = new List<Category>();

        //    defaultCategories.Add(new Category() { CategoryId = 1, Name = "Entrées" });
        //    defaultCategories.Add(new Category() { CategoryId = 2, Name = "Viandes" });
        //    defaultCategories.Add(new Category() { CategoryId = 3, Name = "Poissons" });
        //    defaultCategories.Add(new Category() { CategoryId = 4, Name = "Plats" });
        //    defaultCategories.Add(new Category() { CategoryId = 5, Name = "Accompagnement" });
        //    defaultCategories.Add(new Category() { CategoryId = 6, Name = "Desserts" });

        //    foreach (Category cat in defaultCategories)
        //        context.Categories.Add(cat);

        //    IList<Type> defaultTypes = new List<Type>();

        //    defaultTypes.Add(new Type() { TypeId = 1, Name = "Cakes", CategoryId = 1 });
        //    defaultTypes.Add(new Type() { TypeId = 2, Name = "Salades", CategoryId = 1 });
        //    defaultTypes.Add(new Type() { TypeId = 3, Name = "Soupes", CategoryId = 1 });
        //    defaultTypes.Add(new Type() { TypeId = 4, Name = "Tartes", CategoryId = 1 });
        //    defaultTypes.Add(new Type() { TypeId = 5, Name = "Agneau", CategoryId = 2 });
        //    defaultTypes.Add(new Type() { TypeId = 6, Name = "Boeuf", CategoryId = 2 });
        //    defaultTypes.Add(new Type() { TypeId = 7, Name = "Canard", CategoryId = 2 });
        //    defaultTypes.Add(new Type() { TypeId = 8, Name = "Lapin", CategoryId = 2 });
        //    defaultTypes.Add(new Type() { TypeId = 9, Name = "Porc", CategoryId = 2 });
        //    defaultTypes.Add(new Type() { TypeId = 10, Name = "Poulet", CategoryId = 2 });
        //    defaultTypes.Add(new Type() { TypeId = 11, Name = "Veau", CategoryId = 2 });

        //    foreach (Type t in defaultTypes)
        //        context.Types.Add(t);

        //    base.Seed(context);
        //}
    
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Indiquez votre service de messagerie ici pour envoyer un e-mail.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Connectez votre service SMS ici pour envoyer un message texte.
            return Task.FromResult(0);
        }
    }

    // Configurer l'application que le gestionnaire des utilisateurs a utilisée dans cette application. UserManager est défini dans ASP.NET Identity et est utilisé par l'application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configurer la logique de validation pour les noms d'utilisateur
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configurer la logique de validation pour les mots de passe
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configurer les valeurs par défaut du verrouillage de l'utilisateur
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Inscrire les fournisseurs d'authentification à 2 facteurs. Cette application utilise le téléphone et les e-mails comme procédure de réception de code pour confirmer l'utilisateur
            // Vous pouvez écrire votre propre fournisseur et le connecter ici.
            manager.RegisterTwoFactorProvider("Code téléphonique ", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Votre code de sécurité est {0}"
            });
            manager.RegisterTwoFactorProvider("Code d'e-mail", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Code de sécurité",
                BodyFormat = "Votre code de sécurité est {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configurer le gestionnaire de connexion d'application qui est utilisé dans cette application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
