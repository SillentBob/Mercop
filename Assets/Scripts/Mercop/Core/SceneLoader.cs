using System.Collections.Generic;
using Mercop.Core.Events;
using Mercop.Ui;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mercop.Core
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        private List<string> loadedScenes = new List<string>();
        private List<string> unloadInProgressScenes = new List<string>();
        private string currentGameLevelSceneName;

        protected override void Awake()
        {
            base.Awake();
            EventManager.AddListener<LoadSceneEvent>(OnLoadRequest);
        }

        private void Start()
        {
            ShowMainMenu();
        }

        private void OnLoadRequest(LoadSceneEvent evt)
        {
            if (evt.isLoad)
            {
                if (evt.isMainMenu)
                {
                    ShowMainMenu();
                }
                else
                {
                    LoadLevel(evt.sceneName);
                }
            }
        }

        private void ShowMainMenu()
        {
            Debug.Log("ShowMainMenu");
            ViewManager.Instance.ShowView<MainMenuView>();
            if (loadedScenes.Contains(currentGameLevelSceneName))
            {
                UnloadSceneAsync(currentGameLevelSceneName);
                currentGameLevelSceneName = null;
            }
        }

        private void LoadLevel(string sceneName)
        {
            if (!loadedScenes.Contains(sceneName))
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                loadedScenes.Add(sceneName);
                currentGameLevelSceneName = sceneName;
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
                    loadedScenes.Remove(sceneName);
                    unloadInProgressScenes.Remove(sceneName);
                };
            }
        }
    }
}