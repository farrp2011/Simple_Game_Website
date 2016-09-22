using System;
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
    

    [Authorize]
    public class GamePlaysController : Controller
    {
        private CIS219_LAB2DB db = new CIS219_LAB2DB();
        public const int DEFALT_TOP_PLAYS = 10;
        public const string NUMBER_SEL_NAME = "topPlays";
        public const string RADIO_SEL_NAME = "selectGame";


        // GET: GamePlays
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<GamePlay> query = db.GamePlays.OrderByDescending(GamePlay => GamePlay.score).Take(DEFALT_TOP_PLAYS);
            return View(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string selectGame, int? topPlays)
        {
            if(selectGame == null || topPlays == null  || topPlays < 1)
            {
                return new HttpNotFoundResult();
            }

            IEnumerable<GamePlay> query;
            if (selectGame.Equals(GamesController.ALL_GAMES))//If all games
            {
                
                query = db.GamePlays.OrderByDescending(GamePlay => GamePlay.score).Take((int)topPlays);
                return View(query);
            }
            //else display only one game
            query = db.GamePlays.OrderByDescending(GamePlay => GamePlay.score).Take((int)topPlays).Where(GamePlay => GamePlay.gameName.Equals(selectGame));
            return View(query);


        }

        
        // GET: GamePlays/Create
        public ActionResult Create()
        {
            return RedirectToAction("Index");
        }

        // POST: GamePlays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,gameName,score,playerName,timeStamp")] GamePlay gamePlay)
        {
            if (ModelState.IsValid)
            {
                db.GamePlays.Add(gamePlay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gamePlay);
        }
       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
