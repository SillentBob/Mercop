namespace Core.Events
{
    public class LoadSceneEvent
    {
        public string sceneName;
        public bool isMainMenu;
        
        public LoadSceneEvent(string sceneName)
        {
            this.sceneName = sceneName;
        }
        public LoadSceneEvent(bool isMainMenu)
        {
            this.isMainMenu = isMainMenu;
        }
    }
}