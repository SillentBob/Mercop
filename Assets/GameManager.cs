using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public MissionContractAttributes selectedContract;
    public PlayerSettingsAttributes playerSettings;

    private bool isGamePaused;
    
    /// <returns>true if game is paused, false if resumed</returns>
    public bool PauseOrResumeGame()
    {
        if (!isGamePaused)
        {
            //InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
            Time.timeScale = 0;
        }
        else
        {
            //InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
            Time.timeScale = 1;
        }

        isGamePaused = !isGamePaused;
        return isGamePaused;
    }
}