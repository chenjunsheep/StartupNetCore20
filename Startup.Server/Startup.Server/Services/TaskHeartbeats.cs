using Shared.Api.Schedule.Instance;

namespace Startp.Server.Services
{
    public class TaskHeartbeats : TaskHeartbeat
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="domain">http domain</param>
        /// <param name="frequency">schedule excuting frequency in seconds</param>
        public TaskHeartbeats(string domain, int? frequency) : base(domain, frequency) { }

        public override void Log()
        {
            LogProvider.Log($"Heart Beats");
        }
    }
}
