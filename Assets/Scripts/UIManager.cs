using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Score")]
    public TextMeshProUGUI scoreText;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("Power Bar")]
    public Slider powerBar;
    public GameObject powerBarObj;
    public BowController bowController;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (powerBarObj != null)
            powerBarObj.SetActive(false);
    }

    void Update()
    {
        if (bowController == null || powerBarObj == null) return;

        bool holding = bowController.IsHolding();
        powerBarObj.SetActive(holding);

        if (holding && powerBar != null)
            powerBar.value = bowController.GetPowerPercent();
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void ShowGameOver(int score)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + score;
    }

    public void OnPlayAgainClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RestartGame();
    }
}
