﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    public static GameOverHandler instance;

    [TextArea(2,4)]
    [SerializeField] private string strikeOutGameOverText;
    [TextArea(2,4)]
    [SerializeField] private string timeOutGameOverText;
    [SerializeField] private string scoreTextBegin = "Your Score : ";
    [SerializeField] private int totalLevelCount = 3;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverHeaderText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject nextLevelButton;
    public event Action<GameOverType> OnGameOver;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

    }

    public void TriggerGameOver(GameOverType gameOverType)
    {
        switch (gameOverType)
        {
            case GameOverType.TimeOut:
                MusicController.isGameOver = true;
                AkSoundEngine.PostEvent("Kill_Music", gameObject);
                AkSoundEngine.PostEvent("EndWhistle", gameObject);
                AkSoundEngine.SetSwitch("MusicType", "Win", gameObject);
                AkSoundEngine.PostEvent("Music_End", gameObject);
                gameOverHeaderText.text = timeOutGameOverText;
                nextLevelButton.SetActive(SceneManager.GetActiveScene().buildIndex < totalLevelCount);
                gameOverHeaderText.color = Color.white;
                break;
            case GameOverType.StrikeOut:
                MusicController.isGameOver = true;
                AkSoundEngine.PostEvent("Kill_Music", gameObject);
                AkSoundEngine.SetSwitch("MusicType", "Lose", gameObject);
                AkSoundEngine.PostEvent("Music_End", gameObject);
                gameOverHeaderText.text = strikeOutGameOverText;
                nextLevelButton.SetActive(false);
                gameOverHeaderText.color = Color.red;
                break;
        }

        scoreText.text = scoreTextBegin + ScoreManager.instance.GetScore();
        gameOverPanel.SetActive(true);
        OnGameOver?.Invoke(gameOverType);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (sceneIndex > totalLevelCount) return;
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
