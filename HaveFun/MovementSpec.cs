using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
             game.Handle(new StartGame());
            game.ApplyEvents();
           game.Handle(new MovePlayerOne(0,0));
            game.Handle(new MovePlayerOne(0,1));
            var result = game.Handle(new MovePlayerOne(0,2));
            Assert.Equal(typeof(PlayerOneWins), result.GetType());
        }

        [Fact]
        public void Test3()
        {
            var game = new Game();
            Assert.Throws<InvalidOperationException>(() => game.Handle(new MovePlayerOne(0,0)));
        }
    }

    public class MovePlayerOne : Command
    {
        public int X { get; }
        public int Y { get; }

        public MovePlayerOne(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class GameStarted : Event
    {
    }

    public class Game
    {
        private List<Event> _unstoredEvents = new List<Event>();
        private bool _gameStarted;

        public Event Handle(StartGame startGame)
        {
            var gameStarted = new GameStarted();
            _unstoredEvents.Add(gameStarted);
            
            return gameStarted;
        }

        public Event Handle(MovePlayerOne cmd)
        {
            if(!_gameStarted) throw new InvalidOperationException();
            var playerOneWins = new PlayerOneWins();
            _unstoredEvents.Add(playerOneWins);
            return playerOneWins;
        }

        public void ApplyEvents()
        {
            foreach (var unstoredEvent in _unstoredEvents)
            {
                Apply((dynamic)unstoredEvent);
            }
        }

        public void Apply(GameStarted e)
        {
            _gameStarted = true;
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

