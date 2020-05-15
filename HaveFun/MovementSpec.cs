using System;
using Xunit;

namespace HaveFun
{
    public class OXOTest
    {
        [Fact]
        public void Test1()
        {
            var game = new Game();
            var player1Wins = game.Handle(new StartGame());
            Assert.Equal(typeof(PlayerOneWins), player1Wins.GetType());
        }
    }

    public class Game
    {
        public Event Handle(StartGame startGame)
        {
            return new PlayerOneWins();
        }
    }

    public class PlayerOneWins : Event
    {
    }

    public class Event
    {
    }

    public class StartGame
    {
    }
}

