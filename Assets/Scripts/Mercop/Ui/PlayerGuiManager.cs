using Mercop.Core;
using Mercop.Core.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mercop.Ui
{
    public class PlayerGuiManager : Singleton<PlayerGuiManager>
    {
        // @formatter:off
        [Header("Player GUI")]
        [SerializeField] private Button pauseButton;
        [SerializeField] private Image crosshairIcon;
        [SerializeField] private PlayerGui playerGuiRoot;
        
        [Header("FPS Display")]
        [SerializeField] private bool showFps;
        [SerializeField] private TextMeshProUGUI fpsText;
        // @formatter:on

        private float fpsDeltaTime;

#if UNITY_EDITOR
        private bool lastShowFps;
#endif

        protected override void Awake()
        {
            base.Awake();
            pauseButton.onClick.AddListener(() =>
                EventManager.Invoke(new PauseEvent(true)));
        }

        private void Update()
        {
            if (showFps)
            {
                fpsDeltaTime += (Time.unscaledDeltaTime - fpsDeltaTime) * 0.1f;
                var fps = 1.0f / fpsDeltaTime;
                fpsText.SetText(Mathf.Ceil(fps).ToString());
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

        public void ShowGui(bool show)
        {
            playerGuiRoot.gameObject.SetActive(show);
        }

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
    }
}