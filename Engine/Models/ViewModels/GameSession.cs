using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Models.ViewModels
{
    public class GameSession
    {
        Player CurrentPlayer {get; set;}

        public GameSession(Player currentPlayer)
        {
            CurrentPlayer = new Player();
            CurrentPlayer.Name = "Player 1";
            CurrentPlayer.Assets = 10000;
        }
    }
}