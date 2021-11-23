using System.Collections;
using Core;
using Core.Events;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float quitGameDelay = 0.5f;
    public MissionContractAttributes selectedContract;
    public PlayerSettingsAttributes playerSettings;

    private bool isGamePaused;

    protected override void Awake()
    {
        base.Awake();
        EventManager.AddListener(EventTypes.Pause, OnPauseChange);
        EventManager.AddListener(EventTypes.Quit, OnGameQuit);
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