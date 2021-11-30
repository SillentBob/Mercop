namespace Mercop.Core.Events
{
    public class ContractProgressChange : BaseEvent
    {
        public ContractProgressTracker tracker;
        public bool incrementStep;
        public bool isCompleted;

        public ContractProgressChange(ContractProgressTracker tracker, bool incrementStep, bool isCompleted)
        {
            this.tracker = tracker;
            this.incrementStep = incrementStep;
            this.isCompleted = isCompleted;
        }
    }
}