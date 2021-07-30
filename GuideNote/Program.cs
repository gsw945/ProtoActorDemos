using Proto;
using System;
using System.Threading.Tasks;

namespace GuideNote
{
    #region https://proto.actor/docs/getting-started/
    public class Greet
    {
        public Greet(string who)
        {
            Who = who;
        }

        public string Who { get; private set; }
    }

    public class GreetingActtor : IActor
    {
        public Task ReceiveAsync(IContext ctx)
        {
            if (ctx.Message is Greet greet)
            {
                Console.WriteLine($"Hello {greet.Who}");
            }

            return Task.CompletedTask;
        }
    }
    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            var system = new ActorSystem();
            var props = Props.FromProducer(() => new GreetingActtor());
            var greeter = system.Root.Spawn(props);

            system.Root.Send(greeter, new Greet("World"));

            Console.Write("Press [Enter] key to quit...");
            // This prevents the app from exiting before the async work is done
            Console.ReadLine();
        }
    }
}
