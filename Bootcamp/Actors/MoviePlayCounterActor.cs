using Bootcamp.Messages;
using Proto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Actors
{
    public class MoviePlayCounterActor : IActor
    {
        private Dictionary<string, int> _moviePlayCounts = new Dictionary<string, int>();
        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case IncrementPlayCountMessage msg:
                    ProcessIncrementPlayCountMessage(msg);
                    break;
                default:
                    break;
            }
            return Task.CompletedTask;
        }

        private void ProcessIncrementPlayCountMessage(IncrementPlayCountMessage message)
        {
            if (!_moviePlayCounts.ContainsKey(message.MovieTitle))
            {
                _moviePlayCounts.Add(message.MovieTitle, 0);
            }
            _moviePlayCounts[message.MovieTitle]++;

            ColorConsole.WriteMagenta($"MoviePlayerCountActor '{message.MovieTitle}' has been watched {_moviePlayCounts[message.MovieTitle]} items");
        }
    }
}
