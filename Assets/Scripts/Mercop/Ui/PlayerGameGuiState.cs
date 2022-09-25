using Mercop.Audio;
using Mercop.Core;
using Mercop.Core.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mercop.Ui
{
    public class PlayerGameGuiState : SingletonState<PlayerGameGuiState>
    {
        // @formatter:off
        [Header("Player GUI")]
        [SerializeField] private Button pauseButton;
        [SerializeField] private Image crosshairIcon;
        [SerializeField] private PlayerGui playerGuiRoot;
        [SerializeField] private Button engineStartButton;
        [SerializeField] private Button engineStopButton;
        
        [Header("FPS Gui Display")]
        [SerializeField] private bool showFps;
        [SerializeField] private TextMeshProUGUI fpsText;
        // @formatter:on

        private float fpsDeltaTime;
        private float fps;
        private bool lastShowFps;

        protected override void Awake()
        {
            base.Awake();
            ShowGui(false);
            RegisterButtonListeners();
            RegisterEventListeners();
            SetupButtons();
        }

        private void Update()
        {
            if (showFps)
            {
                fpsDeltaTime += (Time.unscaledDeltaTime - fpsDeltaTime) * 0.1f;
                fps = 1.0f / fpsDeltaTime;
                fpsText.SetText(Mathf.Ceil(fps).ToString());
            }
        }
        
        public override void OnStateEnter()
        {
            ShowGui(true);
            //AudioPlayer.Instance.Play(AudioPlayer.Sound.Music);
        }

        public override void OnStateExit()
        {
            ShowGui(false);
            //AudioPlayer.Instance.Stop(AudioPlayer.Sound.Music);
        }

        private void RegisterButtonListeners()
        {
            pauseButton.onClick.AddListener(OnPauseClick);
        }

        private void RegisterEventListeners()
        {
            EventManager.AddListener<EngineEvent>(OnEngineEvent);
        }

        private void SetupButtons()
        {
            engineStartButton.interactable = true;
            engineStopButton.interactable = false;
        }

        private void OnEngineEvent(EngineEvent evt)
        {
            switch (evt.engineEventType)
            {
                case EngineEvent.EngineEventType.StartBegin:
                    engineStartButton.interactable = false;
                    engineStopButton.interactable = false;
                    break;
                case EngineEvent.EngineEventType.StartFinish:
                    engineStartButton.interactable = false;
                    engineStopButton.interactable = true;
                    break;
                case EngineEvent.EngineEventType.StopBegin:
                    engineStartButton.interactable = false;
                    engineStopButton.interactable = false;
                    break;
                case EngineEvent.EngineEventType.StopFinish:
                    engineStartButton.interactable = true;
                    engineStopButton.interactable = false;
                    break;
            }
        }

        public void MoveCrosshair(Vector2 input, Vector2 sensitivity, Vector2 moveRange)
        {
            crosshairIcon.transform.localPosition = GetCrosshairClampedPosition(input, sensitivity, moveRange);
        }

        private Vector3 GetCrosshairClampedPosition(Vector2 input, Vector2 sensitivity, Vector2 moveRange)
        {
            var localPosition = crosshairIcon.transform.localPosition;
            var xPos = Mathf.Clamp(localPosition.x + input.x * sensitivity.x, -moveRange.x,
                moveRange.x);
            var yPos = Mathf.Clamp(localPosition.y + input.y * sensitivity.y, -moveRange.y,
                moveRange.y);
            return new Vector3(xPos, yPos, 0);
        }

        private void ShowGui(bool show)
        {
            playerGuiRoot.gameObject.SetActive(show);
        }

        private void OnPauseClick()
        {
            EventManager.Invoke(new PauseEvent(true));
            StatesManager.Instance.LoadState<PauseMenuState>();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (lastShowFps != showFps)
            {
                lastShowFps = showFps;
                if (fpsText != null)
                {
                    fpsText.enabled = lastShowFps;
                }
            }
        }
#endif
        
    }
}