using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace CIS219_LAB2.Models
{
    public class GameOverHelper
    {
        private const string DEFALT_USER_NAME = "Guest";
        public const string MONSTER_W_CANDY_GAME_NAME = "Monster Wants Candy";
        public const string SOLO_PONG_GAME_NAME = "Solo Pong";

        /*
         This returns the user name or "Guest" if they are not logged in
         */
        public static string getUserName(bool IsAuthenticated, string userName)
        {
            if (IsAuthenticated)
            {
                return userName;
            }
            else
            {
                return DEFALT_USER_NAME;
            }
        }
    }
}