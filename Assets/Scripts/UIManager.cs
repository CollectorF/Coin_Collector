using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameplayUI;
    [SerializeField]
    private GameObject endUI;

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI winLoseText;

    private void Start()
    {
        ActivateGameplayUI();
        SetScore(0);
    }

    public void SetScore(int score)
    {
        scoreText.text = $"Score {score}";
    }

    public void SetLevelEndText(GameState state)
    {
        switch (state)
        {
            case GameState.Win:
                winLoseText.text = "You win!";
                break;
            case GameState.Lose:
                winLoseText.text = "You lose!";
                break;
            default:
                break;
        }
    }

    public void ActivateGameplayUI()
    {
        gameplayUI.SetActive(true);
        endUI.SetActive(false);
    }

    public void ActivateLevelEndUI()
    {
        gameplayUI.SetActive(false);
        endUI.SetActive(true);
    }
}
