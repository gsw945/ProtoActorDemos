using Bootcamp.Actors;
using Bootcamp.Messages;
using Proto;
using System;
using System.Threading;

namespace Bootcamp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Demo01();
            Demo02();

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

        private class Decider
        {
            public static SupervisorDirective Decide(PID pid, Exception reason)
            {
                return SupervisorDirective.Restart;
            }
        }
    }
}
