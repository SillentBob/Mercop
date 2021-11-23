using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mercop.Core.Events;
using Mercop.Mission;
using Mercop.Player;
using UnityEngine;

namespace Mercop.Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private float quitGameDelay = 0.5f;
        [SerializeField] private List<MissionContractAttributes> allContracts;
        public MissionContractAttributes selectedContract;
        public PlayerSettingsAttributes playerSettings;
        public PlayerResourcesAttributes playerResources;

        private bool isGamePaused;

        protected override void Awake()
        {
            base.Awake();
            EventManager.AddListener(EventTypes.Pause, OnPauseChange);
            EventManager.AddListener(EventTypes.Quit, OnGameQuit);
        }

        public List<MissionContractAttributes> getAvailableContracts()
        {
            return allContracts.Where(m => !m.completed).ToList();;
        }
    
        private void OnPauseChange(PauseEvent evt)
        {
            isGamePaused = evt.isPaused;
            if (isGamePaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        private void OnGameQuit(QuitGameEvent evt)
        {
            StartCoroutine(QuitRoutine());
        }

        private IEnumerator QuitRoutine()
        {
            Debug.Log("Application.Quit()");
            yield return new WaitForSeconds(quitGameDelay);
            Application.Quit();
        }
    }
}