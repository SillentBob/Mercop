using Core;
using Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class PlayerGuiManager : Singleton<PlayerGuiManager>
    {
        // @formatter:off
        [Header("Player GUI")]
        [SerializeField] private Button pauseButton;
        [SerializeField] private Image crosshairIcon;
        [SerializeField] private PlayerGui playerGuiRoot;
    
        // @formatter:on

        protected override void Awake()
        {
            base.Awake();
            pauseButton.onClick.AddListener(() =>
                EventManager.Invoke(EventTypes.Pause, new PauseEvent(true)));
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
    }
}