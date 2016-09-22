using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIS219_LAB2.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        const string ERROR_STR = "Error";
        public const string ALL_GAMES = "ALL";



        // GET: Games/monsterWantsCandy
        public ActionResult monsterWantsCandy()
        {
            return View();
        }

        // GET: Games/soloPong
        public ActionResult soloPong()
        {
            return View();
        }

        // GET: Games/gameover


        /*
         <form id="gameOver" name="gameOver" action='/Games/GameOver' method='post' style="display:none;">
            <input name="__RequestVerificationToken" type="hidden" value="eXsqhiMlM0A7oMG_HSWZ2tI54pT3ZUhGfd1Fz9T7Y8utL_EiBwLXC8Vpcm37gJlAczvTSf81K8SW_Qgx9Wtx59Fqy-ifxwS2mkPsbXli9Vk1" />
            <input type="hidden" id="GameName" name="GameName" value="Monster Wants Candy" />
            <input type="hidden" id="Score" name="Score" value="" />
            <input type="hidden" id="PlayerName" name="PlayerName" value="guest" />
            <input type="hidden" id="WhenScored" name="WhenScored" value="" />
            <input type="submit" id="done" name="done" value="done" style="display:none;" />
        </form>
       */

    }
}