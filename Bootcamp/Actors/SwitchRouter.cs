﻿using Bootcamp.Messages;
using Proto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Actors
{
    public class SwitchRouter : IActor
    {
        private readonly PID _normalFlow;
        private readonly PID _cleanUp;
        private readonly Behavior _behavior;

        public Task ReceiveAsync(IContext context) => _behavior.ReceiveAsync(context);

        public SwitchRouter(PID normalFlow, PID cleanUp)
        {
            _normalFlow = normalFlow;
            _cleanUp = cleanUp;
            _behavior = new Behavior(Off);
        }

        private Task On(IContext context)
        {
            switch (context.Message)
            {
                case Started _:
                    break;
                case RouteStateOn msg:
                    Console.WriteLine("Received on while already in on state");
                    break;
                case RouteStateOff msg:
                    _behavior.Become(Off);
                    break;
                default:
                    context.Forward(_normalFlow);
                    break;
            }
            return Task.CompletedTask;
        }

        private Task Off(IContext context)
        {
            switch (context.Message)
            {
                case Started _:
                    break;
                case RouteStateOn msg:
                    _behavior.Become(On);
                    break;
                case RouteStateOff msg:
                    Console.WriteLine("Received off while already in off state");
                    break;
                default:
                    context.Forward(_cleanUp);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
