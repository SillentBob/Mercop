namespace Mercop.Core.Events
{
    public class PauseEvent
    {
        public bool isPaused;

        public PauseEvent(bool isPaused)
        {
            this.isPaused = isPaused;
        }
    }
}