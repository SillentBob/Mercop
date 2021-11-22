using UnityEngine;
using UnityEngine.UI;

public class PlayerGuiManager : Singleton<PlayerGuiManager>
{
    // @formatter:off
    [Header("Player GUI")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private float pauseButtonUnpausedAlpha = 0.5f;
    [SerializeField] private float pauseButtonPausedAlpha = 1;
    [SerializeField] private Image crosshairIcon;
    [SerializeField] private Transform playerGuiRoot;
    
    // @formatter:on
    
    protected override void Awake()
    {
        base.Awake();
        pauseButton.onClick.AddListener(() =>
        {
            var isPaused = GameManager.Instance.PauseOrResumeGame();
            pauseButton.image.color = new Color(pauseButton.image.color.r, pauseButton.image.color.g,
                pauseButton.image.color.b, isPaused ? pauseButtonPausedAlpha : pauseButtonUnpausedAlpha);
        });
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