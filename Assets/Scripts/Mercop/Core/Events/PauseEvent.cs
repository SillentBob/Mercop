namespace Mercop.Core.Events
{
    public class PauseEvent : BaseEvent
    {
        public bool isPaused;

        public PauseEvent(bool isPaused)
        {
            this.isPaused = isPaused;
        }
    }
}