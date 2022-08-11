using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Win,
    Lose
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;

    private int score = 0;
    private TargetController[] targetControllers;
    private ObstacleController[] obstacleControllers;

    private void Start()
    {
        targetControllers = FindObjectsOfType<TargetController>();
        obstacleControllers = FindObjectsOfType<ObstacleController>();
        foreach (var item in targetControllers)
        {
            item.OnCollected += UpdateScore;
        }
        foreach (var item in obstacleControllers)
        {
            item.OnCollided += LoseGame;
        }
    }

    private void UpdateScore()
    {
        score++;
        uiManager.SetScore(score);
        if (score == targetControllers.Length)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        uiManager.SetLevelEndText(GameState.Win);
        uiManager.ActivateLevelEndUI();
    }

    private void LoseGame()
    {
        uiManager.SetLevelEndText(GameState.Lose);
        uiManager.ActivateLevelEndUI();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
