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
        public void PLayerOneWins()
        {
            var game = new Game();
            game.Handle(new StartGame());
            game.ApplyEvents();
            game.Handle(new MovePlayerOne(0,0));
            game.ApplyEvents();
            game.Handle(new MovePlayerOne(0,1));
            game.ApplyEvents();
           
            var result = game.Handle(new MovePlayerOne(0,2));
            Assert.Equal(typeof(PlayerOneMoved), result.GetType());
        }

        [Fact]
        public void PlayerOneDoesNotWin()
        {
            var game = new Game();
            game.Handle(new StartGame());
            game.ApplyEvents();
            var result = game.Handle(new MovePlayerOne(0,2));
            Assert.Equal(typeof(PlayerOneMoved), result.GetType());
        }
        
        [Fact]
        public void PlayerTwoDoesNotWin()
        {
            var game = new Game();
            game.Handle(new StartGame());
            game.ApplyEvents();
            var result = game.Handle(new MovePlayerTwo(0,2));
            Assert.Equal(typeof(PlayerTwoMoved), result.GetType());
        }

        [Fact]
        public void Test3()
        {
            var game = new Game();
            Assert.Throws<InvalidOperationException>(() => game.Handle(new MovePlayerOne(0,0)));
        }
    }

    public class MovePlayerTwo : Command
    {
        public int X { get; }
        public int Y { get; }

        public MovePlayerTwo(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class PlayerOneMoved : Event
    {
        public int X { get; }
        public int Y { get; }

        public PlayerOneMoved(int x, int y)
        {
            X = x;
            Y = y;
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
    
    // command -> thing -> event
    
    // loading: List<Event> -> apply them all
    // get all events for object
    // apply them all -> current state
    // handle command
    // store new events

    public class GameStarted : Event
    {
    }

    public class Game
    {
        private readonly List<Event> _unstoredEvents = new List<Event>();
        private bool _gameStarted = false;
        private int _playerOneScore = 0;
        private readonly int[,] _board = new int[3,3];

        public Event Handle(StartGame startGame)
        {
            var gameStarted = new GameStarted();
            _unstoredEvents.Add(gameStarted);
            
            return gameStarted;
        }

        public Event Handle(MovePlayerOne cmd)
        {
            if(!_gameStarted) throw new InvalidOperationException();

            if (IsTouching(cmd.X, cmd.Y, 1))
            {
                _playerOneScore = _playerOneScore++;
            }

            if (_playerOneScore < 2)
            {
                return new PlayerOneMoved(cmd.X, cmd.Y);
            }
            
            var playerOneWins = new PlayerOneWins();
            _unstoredEvents.Add(playerOneWins);
            return playerOneWins;
        }
        
        public Event Handle(MovePlayerTwo cmd)
        {
            if(!_gameStarted) throw new InvalidOperationException();

            if (IsTouching(cmd.X, cmd.Y, 2))
            {
                _playerOneScore = _playerOneScore++;
            }

            if (_playerOneScore < 2)
            {
                return new PlayerTwoMoved(cmd.X, cmd.Y);
            }
            
            var playerOneWins = new PlayerTwoWins();
            _unstoredEvents.Add(playerOneWins);
            return playerOneWins;
        }

     

        private bool IsTouching(in int x, in int y, int player)
        {
            var lowx = x - 1;
            var lowy = y - 1;
            var highx = x + 1;
            var highY = y + 1;
            
            if (lowx < 0) lowx = 0;
            if (lowy < 0) lowy = 0;
            if (highx > 2) highx = 2;
            if (highY > 2) highY = 2;

            for (int i = lowx; i <= highx; i++)
            {
                for (int j = lowy; j <= highY; j++)
                {
                    if (i == x && j == y) continue;

                    if (_board[i, j] == player) return true;
                }
            }

            return false;
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

        public void Apply(PlayerOneMoved e)
        {
            _board[e.X, e.Y] = 1;
        }

     
    }
    public class PlayerTwoWins : Event
    {
    }
    public class PlayerTwoMoved : Event
    {
        public int X { get; }
        public int Y { get; }

        public PlayerTwoMoved(in int x, in int y)
        {
            X = x;
            Y = y;
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

