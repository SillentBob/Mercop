using Core;
using Core.Events;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public MissionContractAttributes selectedContract;
    public PlayerSettingsAttributes playerSettings;

    private bool isGamePaused;

    protected override void Awake()
    {
        base.Awake();
        EventManager.AddListener(EventTypes.Pause, OnPauseChange);
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
    
}