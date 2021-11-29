namespace Mercop.Core.Events
{
    public class EngineEvent : BaseEvent
    {
        public EngineEventType engineEventType;
        public bool isActionFinished;
        public EngineEvent(EngineEventType type)
        {
            this.engineEventType = type;
        }
        
        public enum EngineEventType
        {
            StartBegin, StartFinished, StopBegin, StopFinish
        }
    }
}