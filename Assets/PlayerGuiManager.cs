using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGuiManager : MonoBehaviour
{
    [SerializeField] private Button pauseButton;

    private void Awake()
    {
        pauseButton.onClick.AddListener(() =>
        {
            var isPaused = GameManager.Instance.PauseOrResumeGame();
            pauseButton.image.color = new Color(pauseButton.image.color.r, pauseButton.image.color.g,
                pauseButton.image.color.b, isPaused ? 1 : 0.5f);
        });
    }
}