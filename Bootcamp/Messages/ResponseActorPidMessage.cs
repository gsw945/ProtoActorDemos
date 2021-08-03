using Proto;

namespace Bootcamp.Messages
{
    public class ResponseActorPidMessage
    {
        public PID Pid { get; }

        public ResponseActorPidMessage(PID pid)
        {
            Pid = pid;
        }
    }
}
