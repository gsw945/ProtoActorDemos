using Proto;
using System;
using System.Threading.Tasks;

namespace Bootcamp.Actors
{
    public class CleanUpActor : IActor
    {
        public Task ReceiveAsync(IContext context)
        {
            var msg = context.Message;
            if (msg is string r)
            {
                Console.WriteLine($"Cleanup Actor get a message: {r}");
            }
            return Task.CompletedTask;
        }
    }
}
