using Bootcamp.Messages;
using Proto;
using System;
using System.Threading.Tasks;

namespace Bootcamp.Actors
{
    public delegate Task Receive(IContext context);

    public class UserActor : IActor
    {
        private string _currentlyWatching;

        private int _id;

        private readonly PID _moviePlayCounterActorRef;

        public UserActor(int id, PID moviePlayCounterActorRef)
        {
            _moviePlayCounterActorRef = moviePlayCounterActorRef;
            _id = id;
            Console.WriteLine("Creating a UserActor");
            _behavior = new Behavior(Stopped);
        }

        public UserActor(int id)
        {
            _id = id;
            Console.WriteLine("Creating a UserActor");
            _behavior = new Behavior(Stopped);
        }

        private readonly Behavior _behavior;

        public UserActor()
        {
            Console.WriteLine("Creating a UserActor");
            // ColorConsole.WriteLineCyan("Setting initial behavior to stopped");
            _behavior = new Behavior(Stopped);
        }

        public Task ReceiveAsync(IContext context) => _behavior.ReceiveAsync(context);

        private Task Playing(IContext context)
        {
            switch (context.Message)
            {
                case PlayMovieMessage msg:
                    ColorConsole.WriteLineRed("Error: cannot start playing another movie before stopping existing one");
                    break;
                case StopMovieMessage msg:
                    ColorConsole.WriteLineYellow($"User has stopped watching '{_currentlyWatching}'");
                    _currentlyWatching = null;
                    _behavior.Become(Stopped);
                    break;
                default:
                    break;
            }
            ColorConsole.WriteLineCyan("UserActor has now become Playing");

            return Task.CompletedTask;
        }

        private Task Stopped(IContext context)
        {
            switch (context.Message)
            {
                case PlayMovieMessage msg:
                    _currentlyWatching = msg.MovieTitle;
                    ColorConsole.WriteLineYellow($"User is currently watching '{_currentlyWatching}'");
                    context.Send(_moviePlayCounterActorRef, new IncrementPlayCountMessage(_currentlyWatching));
                    _behavior.Become(Playing);
                    break;
                case StopMovieMessage msg:
                    ColorConsole.WriteLineRed("Error: cannot stop if nothing is playing");
                    break;
                default:
                    ColorConsole.WriteLineCyan("UserActor has now become Stopped");
                    break;
            }
            ColorConsole.WriteLineCyan("UserActor has now become Stopped");

            return Task.CompletedTask;
        }
    }
}
