using Bootcamp.Actors;
using Bootcamp.Messages;
using Nito.AsyncEx;
using Proto;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bootcamp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Demo01();
            // Demo02();
            // Demo03();
            // Demo04();
            Demo05();

            Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.Write("Press [Enter] key to quit...");
            Console.ReadLine();
        }

        static void Demo01()
        {
            var system = new ActorSystem();
            Console.WriteLine("Actor system created");

            // var props = Props.FromProducer(() => new PlaybackActor());
            var props = Props.FromProducer(() => new PlaybackActor())
                .WithChildSupervisorStrategy(new OneForOneStrategy(Decider.Decide, 1, null));
            var pid = system.Root.Spawn(props);
            /*
            system.Root.Send(pid, "The Movie");
            system.Root.Send(pid, 44);
            */
            system.Root.Send(pid, new PlayMovieMessage("The Movie", 44));
            system.Root.Send(pid, new PlayMovieMessage("The Movie 2", 54));
            system.Root.Send(pid, new PlayMovieMessage("The Movie 3", 64));
            system.Root.Send(pid, new PlayMovieMessage("The Movie 4", 74));

            Thread.Sleep(50);
            Console.WriteLine("press any key to restart actor");
            Console.ReadLine();

            system.Root.Send(pid, new Recoverable());

            Thread.Sleep(50);
            Console.WriteLine("press any key to stop actor");
            Console.ReadLine();
            system.Root.Stop(pid);
        }

        static void Demo02()
        {
            var system = new ActorSystem();
            Console.WriteLine("Actor system created");

            var props = Props.FromProducer(() => new PlaybackActor());
            var pid = system.Root.Spawn(props);

            system.Root.Send(pid, new PlayMovieMessage("The Movie", 44));
            
            system.Root.Poison(pid);
        }

        static void Demo03()
        {
            var system = new ActorSystem();
            Console.WriteLine("Actor system created");

            var props = Props.FromProducer(() => new UserActor());
            var pid = system.Root.Spawn(props);

            Console.ReadKey();
            Console.WriteLine("Sending PlayMovieMessage (The Movie)");
            system.Root.Send(pid, new PlayMovieMessage("The Movie", 44));
            Console.ReadKey();
            Console.WriteLine("Sending another PlayMovieMessage (The Movie 2)");
            system.Root.Send(pid, new PlayMovieMessage("The Movie 2", 54));

            Console.ReadKey();
            Console.WriteLine("Sending a StopMovieMessae");
            system.Root.Send(pid, new StopMovieMessage(-1));

            Console.ReadKey();
            Console.WriteLine("Sending another StopMovieMessage");
            system.Root.Send(pid, new StopMovieMessage(-1));
        }

        static void Demo04()
        {
            AsyncContext.Run(async () =>
            {
                var system = new ActorSystem();
                Console.WriteLine("Actor system created");

                var props = Props.FromProducer(() => new PlaybackActor());
                var playbackPid = system.Root.Spawn(props);

                var actorPidMessage = await system.Root.RequestAsync<ResponseActorPidMessage>(playbackPid, new RequestActorPidMessage());
                var userCoordinatorActorPid = actorPidMessage.Pid;

                do
                {
                    ShortPause();

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    ColorConsole.WriteLineGray("enter a command and hit enter");

                    var command = Console.ReadLine();

                    if (command != null)
                    {
                        if (command.StartsWith("play"))
                        {
                            var userId = int.Parse(command.Split(',')[1]);
                            var movieTitle = command.Split(',')[2];

                            system.Root.Send(userCoordinatorActorPid, new PlayMovieMessage(movieTitle, userId));
                        }
                        else if (command.StartsWith("stop"))
                        {
                            var userId = int.Parse(command.Split(',')[1]);

                            system.Root.Send(userCoordinatorActorPid, new StopMovieMessage(userId));
                        }
                        else if (command == "exit")
                        {
                            Terminate();
                        }
                    }
                }
                while (true);

                static void ShortPause()
                {
                    Thread.Sleep(250);
                }

                static void Terminate()
                {
                    Console.WriteLine("Actor system shutdown");
                    Console.Write("Press [Enter] key to quit...");
                    Console.ReadLine();
                    Environment.Exit(1);
                }
            });
        }

        static void Demo05()
        {
            var system = new ActorSystem();

            var cleanupProps = Props.FromProducer(() => new CleanUpActor());
            var cleanup = system.Root.Spawn(cleanupProps);

            var normalFlowProps = Props.FromProducer(() => new NormalFlowActor());
            var normalFlow = system.Root.Spawn(normalFlowProps);

            var switchRouterProps = Props.FromProducer(() => new SwitchRouter(normalFlow, cleanup));
            var switchRouter = system.Root.Spawn(switchRouterProps);


            do
            {
                Console.Write("input: ");
                var command = Console.ReadLine();
                if (command != null)
                {
                    if (command.StartsWith("On"))
                    {
                        system.Root.Send(switchRouter, new RouteStateOn());
                    }
                    else if (command.StartsWith("Off"))
                    {
                        system.Root.Send(switchRouter, new RouteStateOff());
                    }
                    else
                    {
                        system.Root.Send(switchRouter, command);
                    }
                }
                Thread.Sleep(50);
                Console.WriteLine();
            }
            while (true);
        }

        private class Decider
        {
            public static SupervisorDirective Decide(PID pid, Exception reason)
            {
                return SupervisorDirective.Restart;
            }
        }
    }
}
