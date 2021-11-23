using System.Collections.Generic;
using Core.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        [SerializeField] private string MainMenuSceneName = "MainMenu";

        private List<string> loadedScenes = new List<string>();
        private List<string> unloadInProgressScenes = new List<string>();
        private string currentGameLevelSceneName;

        protected override void Awake()
        {
            base.Awake();
            EventManager.AddListener(EventTypes.LoadScene, OnLoadRequest);
        }

        private void Start()
        {
            LoadMainMenu();
        }

        private void OnLoadRequest(LoadSceneEvent evt)
        {
            if (evt.isLoad)
            {
                if (evt.isMainMenu)
                {
                    LoadMainMenu();
                }
                else
                {
                    LoadLevel(evt.sceneName);
                }
            }
        }

        private void LoadMainMenu()
        {
            if (!loadedScenes.Contains(MainMenuSceneName))
            {
                SceneManager.LoadScene(MainMenuSceneName, LoadSceneMode.Additive);
                loadedScenes.Add(MainMenuSceneName);
                UnloadSceneAsync(currentGameLevelSceneName);
            }
        }

        private void LoadLevel(string sceneName)
        {
            if (!loadedScenes.Contains(sceneName))
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                loadedScenes.Add(sceneName);
                currentGameLevelSceneName = sceneName;
                UnloadSceneAsync(MainMenuSceneName);
            }
        }

        private void UnloadSceneAsync(string sceneName)
        {
            if (sceneName != null && loadedScenes.Contains(sceneName) && !unloadInProgressScenes.Contains(sceneName))
            {
                unloadInProgressScenes.Add(sceneName);
                AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
                unloadOp.completed += op =>
                {
                    Debug.Log($"UnloadSceneAsync finished");
                    loadedScenes.Remove(sceneName);
                    unloadInProgressScenes.Remove(sceneName);
                };
            }
        }
    }
}