using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mercop.Core.Events;
using Mercop.Core.Web.Data;
using Mercop.Mission;
using Mercop.Player;
using Mercop.Ui;
using UnityEngine;

namespace Mercop.Core
{
    public class GameManager : Singleton<GameManager>
    {
        // @formatter:off
        [SerializeField] private float quitGameDelay = 0.5f;
        [SerializeField] private List<MissionContractAttributes> allContracts;
        public MissionContractAttributes selectedContract;
        public PlayerSettingsAttributes playerSettings;
        public PlayerResourcesAttributes playerResources;
        [Header("FPS")]
        [SerializeField] private bool limitFps;
        [SerializeField, Range(10,120)] private int maxFps = 60;
        [SerializeField] private bool useVsync;
        [Header("WEB")]
        [SerializeField] private DataProvider dataProvider;
        // @formatter:on

        private PlayerAuthData assignedPlayerAuthData;
        private int currentPlayerScore;

        private bool isGamePaused;
#if UNITY_EDITOR
        private bool lastLimitFps;
        private int lastMaxFps;
        private bool lastUseVsync;
#endif

        protected override void Awake()
        {
            base.Awake();
            EventManager.AddListener<PauseEvent>(OnPauseChange);
            EventManager.AddListener<QuitGameEvent>(OnGameQuit);
            EventManager.AddListener<ContractsFinishEvent>(OnContractFinish);
            SetGameFpsSettings(useVsync, limitFps, maxFps);
        }

        public List<MissionContractAttributes> getAvailableContracts()
        {
            return allContracts.Where(m => !m.completed).ToList();
        }

        private void OnContractFinish(ContractsFinishEvent evt)
        {
            selectedContract.completed = true;
            AddPlayerScore(selectedContract.reward.experience);
            SubmitPlayerScore(
                (isFinishSuccesful) => { ViewManager.Instance.ShowView<MainMenuView>(); }
            );
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

        public void GetLeaderboards(Action<LeaderboardsData> onLoadFinish)
        {
            GetPlayerAuthId(pData => { dataProvider.GetLeaderboardsData("PL", pData.idToken, onLoadFinish); });
        }

        public void SubmitPlayerScore(Action<bool> onPostFinish)
        {
            GetPlayerAuthId(pData =>
            {
                dataProvider.PostPlayerData(pData, "ub", playerSettings.playerName, currentPlayerScore,
                    onPostFinish);
            });
        }

        private void GetPlayerAuthId(Action<PlayerAuthData> onGetFinish)
        {
            if (assignedPlayerAuthData != null)
            {
                onGetFinish(assignedPlayerAuthData);
            }
            else
            {
                dataProvider.GetPlayerAuthData(onGetFinish);
            }
        }

        public void AddPlayerScore(int score)
        {
            currentPlayerScore += score;
        }

        private void OnValidate()
        {
            if (lastLimitFps != limitFps)
            {
                lastLimitFps = limitFps;
                if (lastLimitFps)
                {
                    lastUseVsync = useVsync = false;
                    SetGameFpsSettings(false, true, maxFps);
                }
            }

            if (lastUseVsync != useVsync)
            {
                lastUseVsync = useVsync;
                if (lastUseVsync)
                {
                    lastLimitFps = limitFps = false;
                    SetGameFpsSettings(true, false, maxFps);
                }
            }

            if (lastMaxFps != maxFps)
            {
                lastMaxFps = maxFps;
                SetGameFpsSettings(useVsync, limitFps, maxFps);
            }
        }

        private void SetGameFpsSettings(bool useVsyncVal, bool useFpsLimitVal, int maxFpsVal)
        {
            if (useFpsLimitVal)
            {
                useVsync = false;
                limitFps = true;
            }
            else if (useVsyncVal)
            {
                limitFps = false;
                useVsync = true;
            }

            QualitySettings.vSyncCount = useVsync ? 1 : 0;
            Application.targetFrameRate = limitFps && !useVsync ? maxFpsVal : -1;
        }
    }
}