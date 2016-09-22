using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIS219_LAB2.Models;



namespace CIS219_LAB2.Controllers
{

    [Authorize (Roles= AppConst.ADMIN_ROLE)]
    public class AdminController : Controller
    {

        private CIS219_LAB2DB db = new CIS219_LAB2DB();
        private Models.ApplicationDbContext userDB = new ApplicationDbContext();
        

        // GET: GamePlays
        
        public ActionResult Index()
        {
            IEnumerable<GamePlay> query = db.GamePlays.OrderByDescending(GamePlay => GamePlay.score);
            return View(query);
        }

       
        
        [Authorize(Roles = AppConst.ADMIN_ROLE)]
        public ActionResult Prompt()
        {

            IEnumerable<Models.ApplicationUser> query = userDB.Database.SqlQuery<Models.ApplicationUser>("SELECT [Extent1].[Id] AS [Id], [Extent1].[nickName] AS [nickName], [RoleStuff].[Name] AS [role], [Extent1].[avgScoreMWC] AS [avgScoreMWC], [Extent1].[avgSocreSP] AS [avgSocreSP], [Extent1].[nPlaysMWC] AS [nPlaysMWC], [Extent1].[nPlaysSP] AS [nPlaysSP], [Extent1].[highestScoreMWC] AS [highestScoreMWC], [Extent1].[highestScoreSP] AS [highestScoreSP], [Extent1].[losestScoreSP] AS [losestScoreSP], [Extent1].[losestScoreMWC] AS [losestScoreMWC], [Extent1].[Email] AS [Email], [Extent1].[EmailConfirmed] AS [EmailConfirmed], [Extent1].[PasswordHash] AS [PasswordHash], [Extent1].[SecurityStamp] AS [SecurityStamp], [Extent1].[PhoneNumber] AS [PhoneNumber], [Extent1].[PhoneNumberConfirmed] AS [PhoneNumberConfirmed], [Extent1].[TwoFactorEnabled] AS [TwoFactorEnabled], [Extent1].[LockoutEndDateUtc] AS [LockoutEndDateUtc], [Extent1].[LockoutEnabled] AS [LockoutEnabled], [Extent1].[AccessFailedCount] AS [AccessFailedCount], [Extent1].[UserName] AS [UserName] FROM [dbo].[AspNetUsers] AS [Extent1] LEFT JOIN (SELECT * FROM [AspNetRoles] INNER JOIN [AspNetUserRoles] ON [AspNetRoles].[Id]=[AspNetUserRoles].[RoleId]) AS [RoleStuff] ON [Extent1].[Id]=[RoleStuff].[UserId];");

            return View(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppConst.ADMIN_ROLE)]
        public ActionResult Prompt(string userId)
        {
            userId = userId.Replace("'","");//stoping some sql injection!
            //check to see if real user
            //stoping some injection
            Models.ApplicationUser aUser = userDB.Users.Find(userId);
            if(aUser == null)
            {
                return new HttpNotFoundResult();
            }
            //See if Admin or reguler user
            IEnumerable<Models.ApplicationUser> query = userDB.Database.SqlQuery<Models.ApplicationUser>("SELECT [Extent1].[Id] AS [Id], [Extent1].[nickName] AS [nickName], [RoleStuff].[Name] AS [role], [Extent1].[avgScoreMWC] AS [avgScoreMWC], [Extent1].[avgSocreSP] AS [avgSocreSP], [Extent1].[nPlaysMWC] AS [nPlaysMWC], [Extent1].[nPlaysSP] AS [nPlaysSP], [Extent1].[highestScoreMWC] AS [highestScoreMWC], [Extent1].[highestScoreSP] AS [highestScoreSP], [Extent1].[losestScoreSP] AS [losestScoreSP], [Extent1].[losestScoreMWC] AS [losestScoreMWC], [Extent1].[Email] AS [Email], [Extent1].[EmailConfirmed] AS [EmailConfirmed], [Extent1].[PasswordHash] AS [PasswordHash], [Extent1].[SecurityStamp] AS [SecurityStamp], [Extent1].[PhoneNumber] AS [PhoneNumber], [Extent1].[PhoneNumberConfirmed] AS [PhoneNumberConfirmed], [Extent1].[TwoFactorEnabled] AS [TwoFactorEnabled], [Extent1].[LockoutEndDateUtc] AS [LockoutEndDateUtc], [Extent1].[LockoutEnabled] AS [LockoutEnabled], [Extent1].[AccessFailedCount] AS [AccessFailedCount], [Extent1].[UserName] AS [UserName] FROM [dbo].[AspNetUsers] AS [Extent1] LEFT JOIN (SELECT * FROM [AspNetRoles] INNER JOIN [AspNetUserRoles] ON [AspNetRoles].[Id]=[AspNetUserRoles].[RoleId]) AS [RoleStuff] ON [Extent1].[Id]=[RoleStuff].[UserId] WHERE [RoleStuff].[Name] IS NOT NULL AND [Extent1].[Id] ='"+userId+"';");
            if (query.Count() == null || query.Count()==0)//if true this user in not a admin
            {
                userDB.Database.SqlQuery<object>/*Don't care what comes out*/("INSERT INTO [AspNetUserRoles] ([UserId],[RoleId]) VALUES( '"+userId+"',(SELECT [AspNetRoles].[Id] FROM [AspNetRoles] WHERE [AspNetRoles].[Name] = '"+AppConst.ADMIN_ROLE+"'));").ToList();//.ToList() should make the query stick
                userDB.SaveChanges();
            }
            else
            {
                userDB.Database.SqlQuery<object/*I don't care what comes out*/>("DELETE FROM [AspNetUserRoles] WHERE [AspNetUserRoles].[UserId]='" + userId + "';").ToList();
                userDB.SaveChanges();
            }
            //remove/add admin role
            
            query = userDB.Database.SqlQuery<Models.ApplicationUser>("SELECT [Extent1].[Id] AS [Id], [Extent1].[nickName] AS [nickName], [RoleStuff].[Name] AS [role], [Extent1].[avgScoreMWC] AS [avgScoreMWC], [Extent1].[avgSocreSP] AS [avgSocreSP], [Extent1].[nPlaysMWC] AS [nPlaysMWC], [Extent1].[nPlaysSP] AS [nPlaysSP], [Extent1].[highestScoreMWC] AS [highestScoreMWC], [Extent1].[highestScoreSP] AS [highestScoreSP], [Extent1].[losestScoreSP] AS [losestScoreSP], [Extent1].[losestScoreMWC] AS [losestScoreMWC], [Extent1].[Email] AS [Email], [Extent1].[EmailConfirmed] AS [EmailConfirmed], [Extent1].[PasswordHash] AS [PasswordHash], [Extent1].[SecurityStamp] AS [SecurityStamp], [Extent1].[PhoneNumber] AS [PhoneNumber], [Extent1].[PhoneNumberConfirmed] AS [PhoneNumberConfirmed], [Extent1].[TwoFactorEnabled] AS [TwoFactorEnabled], [Extent1].[LockoutEndDateUtc] AS [LockoutEndDateUtc], [Extent1].[LockoutEnabled] AS [LockoutEnabled], [Extent1].[AccessFailedCount] AS [AccessFailedCount], [Extent1].[UserName] AS [UserName] FROM [dbo].[AspNetUsers] AS [Extent1] LEFT JOIN (SELECT * FROM [AspNetRoles] INNER JOIN [AspNetUserRoles] ON [AspNetRoles].[Id]=[AspNetUserRoles].[RoleId]) AS [RoleStuff] ON [Extent1].[Id]=[RoleStuff].[UserId];");

            return View(query);
        }

        /*public string Prompt()//debug code to get SQL statment
        {

            IEnumerable<Models.ApplicationUser> query = userDB.Users.;

            return query.ToString();
        }*/
        public ActionResult EmptyLeaderBorad()
        {
            IEnumerable<GamePlay> query = db.GamePlays.OrderByDescending(GamePlay => GamePlay.score);

            return View(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmptyLeaderBorad(int? id)
        {
            GamePlay play = db.GamePlays.Find(id);
            if(id == null || play == null)//just a little error checking
            {
                return new HttpNotFoundResult();
            }
            


            db.GamePlays.Remove(play);            
            db.SaveChanges();
            IEnumerable<GamePlay> query = db.GamePlays.OrderByDescending(GamePlay => GamePlay.score);
            
            return View(query);
        }

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmptyLeaderBorad(string option)//Everthing Will be gone
        {
            if (option == null || option.Length < 0 || !option.Equals(GamesController.ALL_GAMES))//just a little error checking
            {
                return new HttpNotFoundResult();
            }
            db.GamePlays.SqlQuery(@"DELETE * FROM GamePlays;");//Oh no everything is GONE

            IEnumerable<GamePlay> query = db.GamePlays.OrderByDescending(GamePlay => GamePlay.score).Take(GamePlaysController.DEFALT_TOP_PLAYS);
            return View(query);
        }*/




        [Authorize(Roles = AppConst.ADMIN_ROLE)]
        public ActionResult RemoveUser()
        {

            IEnumerable<Models.ApplicationUser> query = userDB.Database.SqlQuery<Models.ApplicationUser>("SELECT [Extent1].[Id] AS [Id], [Extent1].[nickName] AS [nickName], [RoleStuff].[Name] AS [role], [Extent1].[avgScoreMWC] AS [avgScoreMWC], [Extent1].[avgSocreSP] AS [avgSocreSP], [Extent1].[nPlaysMWC] AS [nPlaysMWC], [Extent1].[nPlaysSP] AS [nPlaysSP], [Extent1].[highestScoreMWC] AS [highestScoreMWC], [Extent1].[highestScoreSP] AS [highestScoreSP], [Extent1].[losestScoreSP] AS [losestScoreSP], [Extent1].[losestScoreMWC] AS [losestScoreMWC], [Extent1].[Email] AS [Email], [Extent1].[EmailConfirmed] AS [EmailConfirmed], [Extent1].[PasswordHash] AS [PasswordHash], [Extent1].[SecurityStamp] AS [SecurityStamp], [Extent1].[PhoneNumber] AS [PhoneNumber], [Extent1].[PhoneNumberConfirmed] AS [PhoneNumberConfirmed], [Extent1].[TwoFactorEnabled] AS [TwoFactorEnabled], [Extent1].[LockoutEndDateUtc] AS [LockoutEndDateUtc], [Extent1].[LockoutEnabled] AS [LockoutEnabled], [Extent1].[AccessFailedCount] AS [AccessFailedCount], [Extent1].[UserName] AS [UserName] FROM [dbo].[AspNetUsers] AS [Extent1] LEFT JOIN (SELECT * FROM [AspNetRoles] INNER JOIN [AspNetUserRoles] ON [AspNetRoles].[Id]=[AspNetUserRoles].[RoleId]) AS [RoleStuff] ON [Extent1].[Id]=[RoleStuff].[UserId];");

            return View(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppConst.ADMIN_ROLE)]
        public ActionResult RemoveUser(string userId)
        {
            userId = userId.Replace("'", "");//stoping some sql injection!
            //check to see if real user
            //stoping some injection
            Models.ApplicationUser aUser = userDB.Users.Find(userId);
            if (aUser == null)
            {
                return new HttpNotFoundResult();
            }
            //See if Admin or reguler user

            db.Database.SqlQuery<object>("DELETE FROM [GamePlays] WHERE [GamePlays].[playerName] = '"+aUser.UserName+"');").ToList();
            db.SaveChanges();

            userDB.Database.SqlQuery<object>/*Don't care what comes out*/("DELETE FROM [AspNetUsers] WHERE [AspNetUsers].[Id]='" + userId + "';").ToList();//.ToList() should make the query stick
            userDB.SaveChanges();

            userDB.Database.SqlQuery<object/*I don't care what comes out*/>("DELETE FROM [AspNetUserRoles] WHERE [AspNetUserRoles].[UserId]='" + userId + "';").ToList();
            userDB.SaveChanges();



            IEnumerable<Models.ApplicationUser> query = userDB.Database.SqlQuery<Models.ApplicationUser>("SELECT [Extent1].[Id] AS [Id], [Extent1].[nickName] AS [nickName], [RoleStuff].[Name] AS [role], [Extent1].[avgScoreMWC] AS [avgScoreMWC], [Extent1].[avgSocreSP] AS [avgSocreSP], [Extent1].[nPlaysMWC] AS [nPlaysMWC], [Extent1].[nPlaysSP] AS [nPlaysSP], [Extent1].[highestScoreMWC] AS [highestScoreMWC], [Extent1].[highestScoreSP] AS [highestScoreSP], [Extent1].[losestScoreSP] AS [losestScoreSP], [Extent1].[losestScoreMWC] AS [losestScoreMWC], [Extent1].[Email] AS [Email], [Extent1].[EmailConfirmed] AS [EmailConfirmed], [Extent1].[PasswordHash] AS [PasswordHash], [Extent1].[SecurityStamp] AS [SecurityStamp], [Extent1].[PhoneNumber] AS [PhoneNumber], [Extent1].[PhoneNumberConfirmed] AS [PhoneNumberConfirmed], [Extent1].[TwoFactorEnabled] AS [TwoFactorEnabled], [Extent1].[LockoutEndDateUtc] AS [LockoutEndDateUtc], [Extent1].[LockoutEnabled] AS [LockoutEnabled], [Extent1].[AccessFailedCount] AS [AccessFailedCount], [Extent1].[UserName] AS [UserName] FROM [dbo].[AspNetUsers] AS [Extent1] LEFT JOIN (SELECT * FROM [AspNetRoles] INNER JOIN [AspNetUserRoles] ON [AspNetRoles].[Id]=[AspNetUserRoles].[RoleId]) AS [RoleStuff] ON [Extent1].[Id]=[RoleStuff].[UserId];");

            return View(query);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                userDB.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}