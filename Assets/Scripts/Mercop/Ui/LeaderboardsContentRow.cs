using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardsContentRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI placeText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    
    public int placeNumber;
    public string playerName;
    public int score;
}
