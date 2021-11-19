using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public MissionContractAttributes selectedContract;
    public PlayerSettingsAttributes playerSettings;

    private bool _isGamePaused;

    /// <summary>
    /// 
    /// </summary>
    /// <returns>true if game is paused, false if resumed</returns>
    public bool PauseOrResumeGame()
    {
        if (!_isGamePaused)
        {
            //InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
            Time.timeScale = 0;
        }
        else
        {
            //InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
            Time.timeScale = 1;
        }

        _isGamePaused = !_isGamePaused;
        return _isGamePaused;
    }
}