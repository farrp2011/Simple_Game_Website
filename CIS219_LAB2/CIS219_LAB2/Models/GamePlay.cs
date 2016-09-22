using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CIS219_LAB2.Models
{
    /*This is for when a player finishes a game it get enterd into the global scorebord*/

    /*  all of this is the form data from the game when a player fininshes
    <form id="gameOver" name="gameOver" action='/Games/GameOver' method='post' style="display:none;">
    <input type="hidden" id="GameName" name="GameName" value="Monster Wants Candy" />
    <input type="hidden" id="Score" name="Score" value="" />
    <input type="hidden" id="PlayerName" name="PlayerName" value="guest" />
    <input type="hidden" id="WhenScored" name="WhenScored" value="" />
    <input type="submit" id="done" name="done" value="done" style="display:none;" />*/

    public class GamePlay
    {
        public virtual long id { get; set; }
        [Display(Name = "Game Name")]
        public virtual string gameName { get; set; }
        [Display(Name = "Score")]
        public virtual int score { get; set; }
        [Display(Name = "Player Name")]
        public virtual string playerName { get; set; }
        [Display(Name = "Time Stamp")]
        public virtual string timeStamp { get; set; }
    }
}