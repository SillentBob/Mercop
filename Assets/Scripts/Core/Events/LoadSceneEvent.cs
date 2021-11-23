namespace Core.Events
{
    public class LoadSceneEvent
    {
        public string sceneName;
        public bool isMainMenu;
        public bool isLoad;
        public bool isGameLevel;
        

        /// <param name="sceneName"></param>
        /// <param name="isMainMenu"></param>
        /// <param name="isLoad"> true if load, false if unload</param>
        /// <param name="isGameLevel"></param>
        public LoadSceneEvent(string sceneName, bool isMainMenu, bool isLoad, bool isGameLevel)
        {
            this.sceneName = sceneName;
            this.isMainMenu = isMainMenu;
            this.isLoad = isLoad;
            this.isGameLevel = isGameLevel;
        }

    }
}