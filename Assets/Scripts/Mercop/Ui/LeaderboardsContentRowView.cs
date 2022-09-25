using TMPro;
using UnityEngine;

public class LeaderboardsContentRowView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI placeText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerScoreText;

    private string playerName;
    private int score;
    private int placeNumber;
    
    public string PlayerName
    {
        get => playerName;
        set
        {
            playerName = value;
            playerNameText.SetText(playerName);
        }
    }

    public int PlaceNumber
    {
        get => placeNumber;
        set
        {
            placeNumber = value;
            placeText.SetText(placeNumber.ToString());
        }
    }

    public int Score
    {
        get => score;
        set
        {
            score = value;
            playerScoreText.SetText(score.ToString());
        }
    }
}