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
            var result = game.Handle(new StartGame());
            Assert.Equal(typeof(GameStarted), result.GetType());
        }
        
        [Fact]
        public void Test2()
        {
            var game = new Game();
            var result = game.Handle(new MovePlayerOne());
            Assert.Equal(typeof(PlayerOneWins), result.GetType());
        }

        [Fact]
        public void asdf()
        {
            
        }
    }

    public class MovePlayerOne : Command
    {
    }

    public class GameStarted : Event
    {
    }

    public class Game
    {
        public Event Handle(StartGame startGame)
        {
            return new GameStarted();
        }

        public Event Handle(MovePlayerOne cmd)
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

    public class StartGame : Command
    {
    }

    public class Command
    {
    }
}

