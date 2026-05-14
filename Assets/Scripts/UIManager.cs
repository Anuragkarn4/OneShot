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
    public TextMeshProUGUI bestScoreText;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalBestScoreText;

    [Header("Power Bar")]
    public Slider powerBar;
    public GameObject powerBarObj;
    public BowController bowController;
    public Image powerBarFill;

    [Header("Power Bar Colors")]
    public Color lowPowerColor = Color.green;
    public Color midPowerColor = Color.yellow;
    public Color highPowerColor = Color.red;

    // How long to show last shot after release
    public float lastShotDisplayTime = 0.5f;
    private float lastShotTimer = 0f;  // drag the Fill child of the Slider here

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
        if (bowController == null || powerBarObj == null || powerBar == null) return;

        bool holding = bowController.IsHolding();

        if (holding)
        {
            // While holding: show current charge
            powerBarObj.SetActive(true);
            float p = bowController.GetPowerPercent();
            powerBar.value = p;
            UpdatePowerBarColor(p);
            lastShotTimer = 0f;
        }
        else
        {
            // Not holding: display last shot for a short time
            float lastP = bowController.GetLastShotPercent();

            if (lastP > 0f && lastShotTimer < lastShotDisplayTime)
            {
                lastShotTimer += Time.deltaTime;
                powerBarObj.SetActive(true);
                powerBar.value = lastP;
                UpdatePowerBarColor(lastP);
            }
            else
            {
                powerBarObj.SetActive(false);
            }
        }
    }

    void UpdatePowerBarColor(float p)
    {
        if (powerBarFill == null) return;

        // 0–0.5: low → mid, 0.5–1: mid → high
        if (p <= 0.5f)
        {
            float t = p / 0.5f; // 0→1
            powerBarFill.color = Color.Lerp(lowPowerColor, midPowerColor, t);
        }
        else
        {
            float t = (p - 0.5f) / 0.5f; // 0→1
            powerBarFill.color = Color.Lerp(midPowerColor, highPowerColor, t);
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void UpdateBestScore(int best)
    {
        if (bestScoreText != null)
            bestScoreText.text = "Best: " + best;
    }


    public void ShowGameOver(int score, int bestScore)
    {
       if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Score: " + score;

        if (finalBestScoreText != null)
            finalBestScoreText.text = "High Score: " + bestScore;
    }

    public void OnPlayAgainClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RestartGame();
    }
}
