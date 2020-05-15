using System;
using Xunit;

namespace HaveFun
{
    public class OXOTest
    {
        [Fact]
        public void Test1()
        {
            bool player1Wins;
            var game = new Game();
            player1Wins = game.Handle(new StartGame());
            Assert.Equal(true, player1Wins);
        }
    }

    public class Game
    {
        public bool Handle(StartGame startGame)
        {
            return true;
        }
    }

    public class StartGame
    {
    }
}

