using Bootcamp.Messages;
using Proto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Actors
{
    public class ChildActor : IActor
    {
        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Restarting msg:
                    ProcessRestartingMessage(msg);
                    break;
                case Recoverable msg:
                    ProcessRecoverableMessage(msg);
                    break;
                default:
                    break;
            }
            return Task.CompletedTask;
        }

        private void ProcessRestartingMessage(Restarting msg)
        {
            ColorConsole.WriteLineGreen("ChildActor Restarting");
        }

        private void ProcessRecoverableMessage(Recoverable msg)
        {
            throw new Exception();
        }
    }
}
