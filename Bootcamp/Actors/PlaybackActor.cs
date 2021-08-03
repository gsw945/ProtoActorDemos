using Bootcamp.Messages;
using Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Actors
{
    public class PlaybackActor : IActor
    {
        private Dictionary<string, PID> _links = new Dictionary<string, PID>();

        private PID _userCoordinatorActorRef;
        private PID _moviePlayCounterActorRef;
        private string _currentlyWatching;

        public PlaybackActor() => Console.WriteLine("Creating a PlaybackActor");

        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Started msg:
                    ProcessStartedMessage(context, msg);
                    break;
                /*
                case string movieTitle:
                    Console.WriteLine($"Received movie title {movieTitle}");
                    break;
                case int userId:
                    Console.WriteLine($"Received user ID {userId}");
                    break;
                */
                case PlayMovieMessage msg:
                    // Console.WriteLine($"Received movie title {msg.MovieTitle}");
                    // Console.WriteLine($"Received user ID {msg.UserId}");
                    ProcessPlayMovieMessage(msg);
                    break;
                case Recoverable msg:
                    ProcessRecoverableMessage(context, msg);
                    break;
                case Stopping msg:
                    ProcessStoppingMessage(msg);
                    break;
                case Stopped msg:
                    Console.WriteLine("actor is stopped");
                    break;
                case RequestActorPidMessage msg:
                    ProcessRequestActorPidMessage(context, msg);
                    break;
                default:
                    break;
            }
            return Task.CompletedTask;
        }

        private void ProcessRequestActorPidMessage(IContext context, RequestActorPidMessage msg)
        {
            context.Respond(new ResponseActorPidMessage(_userCoordinatorActorRef));
        }

        private void ProcessStartedMessage(IContext context, Started msg)
        {
            ColorConsole.WriteLineGreen("PlaybackActor Started");

            var moviePlayCounterActorProps = Props.FromProducer(() => new MoviePlayCounterActor());
            _moviePlayCounterActorRef = context.Spawn(moviePlayCounterActorProps);

            var props = Props.FromProducer(() => new UserCoordinatorActor(_moviePlayCounterActorRef));
            _userCoordinatorActorRef = context.Spawn(props);
        }

        private void ProcessPlayMovieMessage(PlayMovieMessage msg)
        {
            ColorConsole.WriteLineYellow($"PlayMovieMessage {msg.MovieTitle} for user {msg.UserId}");
        }

        private void ProcessRecoverableMessage(IContext context, Recoverable msg)
        {
            PID child;
            if (context.Children == null || context.Children.Count == 0)
            {
                var props = Props.FromProducer(() => new ChildActor());
                child = context.Spawn(props);
            }
            else
            {
                child = context.Children.First();
            }

            context.Forward(child);
        }

        private void ProcessStoppingMessage(Stopping msg)
        {
            ColorConsole.WriteLineGreen("PlaybackActor Stopping");
        }
    }
}
