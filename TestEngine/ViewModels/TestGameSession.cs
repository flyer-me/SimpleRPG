using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.ViewModels;

namespace TestEngine.ViewModels
{
    [TestClass]
    public class TestGameSession
    {
        [TestMethod]
        public void TestCreateGameSession()
        {
            GameSession gameSession = new GameSession();
            Assert.IsNotNull(gameSession.CurrentPlayer);
            Assert.AreEqual("Town square", gameSession.CurrentLocation.Name);
        }
        [TestMethod]
        public void TestPlayerMovesHomeAndIsCompletelyHealedOnKilled()
        {
            GameSession gameSession = new GameSession();
            gameSession.CurrentPlayer.TakeDamage(int.MaxValue);
            Assert.AreEqual("Home", gameSession.CurrentLocation.Name);
            Assert.AreEqual(gameSession.CurrentPlayer.Level * 10, gameSession.CurrentPlayer.CurrentHitPoints);
        }
        [TestMethod]
        public void TestPlayerMoveNorthSouthWestEast()
        {
            GameSession gameSession = new GameSession();
             Assert.AreEqual(gameSession.CurrentWorld.LocationAt(0, 0), gameSession.CurrentLocation);
            gameSession.MoveNorth();
            Assert.AreEqual(gameSession.CurrentWorld.LocationAt(0, 1), gameSession.CurrentLocation);
            gameSession.MoveSouth();
            Assert.AreEqual(gameSession.CurrentWorld.LocationAt(0, 0), gameSession.CurrentLocation);
            gameSession.MoveWest();
            Assert.AreEqual(gameSession.CurrentWorld.LocationAt(-1, 0), gameSession.CurrentLocation);
            gameSession.MoveEast();
            Assert.AreEqual(gameSession.CurrentWorld.LocationAt(0, 0), gameSession.CurrentLocation);
        }
    }
}