using chat.messages;
using Proto;
using Proto.Remote;
using Proto.Remote.GrpcCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Proto.Remote.GrpcCore.GrpcCoreRemoteConfig;

namespace Chat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = BindToLocalhost(port: 8080)
                .WithProtoMessages(ChatReflection.Descriptor);

            var system = new ActorSystem()
                .WithRemote(config);

            system.Remote().StartAsync();

            var context = new RootContext(system);

            var clients = new HashSet<PID>();

            var props = Props.FromFunc(ctx =>
            {
                switch (ctx.Message)
                {
                    case Connect connect:
                        Console.WriteLine($"Client {connect.Sender} connected");
                        clients.Add(connect.Sender);
                        ctx.Send(connect.Sender, new Connected { Message = "Welcome!" });
                        break;

                    case SayRequest sayRequest:
                        Console.WriteLine($"Client {sayRequest.UserName} say: {sayRequest.Message}");
                        foreach (var client in clients)
                        {
                            ctx.Send(
                                client, new SayResponse
                                {
                                    UserName = sayRequest.UserName,
                                    Message = sayRequest.Message
                                }
                            );
                        }
                        break;

                    case NickRequest nickRequest:
                        Console.WriteLine($"Client {nickRequest.OldUserName} change NickName to [{nickRequest.NewUserName}]");
                        foreach (var client in clients)
                        {
                            ctx.Send(
                                client, new NickResponse
                                {
                                    OldUserName = nickRequest.OldUserName,
                                    NewUserName = nickRequest.NewUserName
                                }
                            );
                        }
                        break;

                    default:
                        Console.WriteLine($"default: {ctx.Message}");
                        break;
                }

                return Task.CompletedTask;
            });

            context.SpawnNamed(props, "chatserver");
            Console.WriteLine("Press [Enter] key to exit...");
            Console.ReadLine();
        }
    }
}
