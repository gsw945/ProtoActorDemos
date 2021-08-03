using Proto;
using System.Threading.Tasks;

namespace Bootcamp.Actors
{
    public class EchoActor : IActor
    {
        public Task ReceiveAsync(IContext context)
        {
            context.Respond(context.Message);
            return Task.CompletedTask;
        }
    }
}
