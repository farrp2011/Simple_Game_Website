using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Owin.Security;

namespace CIS219_LAB2.Models
{

    public class AppConst
    {
        public const string ADMIN_ROLE = "Admin";
        public const string GUEST = "GUEST";
    }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display (Name="Nick Name")]
        public virtual string nickName {get;set;}
        [Display(Name = "Role")]
        public virtual string role { get; set; }

        //MWC = Monster Wants Candy
        //SP = Solo Pong

        public virtual float avgScoreMWC { get; set; }
        public virtual float avgSocreSP { get; set; }

        public virtual int nPlaysMWC { get; set; }
        public virtual int nPlaysSP { get; set; }

        public virtual int highestScoreMWC { get; set; }
        public virtual int highestScoreSP { get; set; }

        public virtual int losestScoreSP { get; set; }
        public virtual int losestScoreMWC { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

       
    }
}