using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int bestScore = 0;

    public int bombHits = 0;
    public int maxBombHits = 5;
    public bool isGameOver = false;

    private const string BestScoreKey = "BestScore";

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        //Debug.Log("GM Awake: loaded BestScore = " + bestScore);
    }

    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScore(score);
            UIManager.Instance.UpdateBestScore(bestScore);
        }
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;

        score += points;
        if (score < 0) score = 0;

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.Save();
        }

        UpdateUI();
    }

    public void OnBombHit()
    {
        if (isGameOver) return;

        bombHits++;
        AddScore(-8);

        if (bombHits >= maxBombHits)
            TriggerGameOver();
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;

        bool isNewBest = score > PlayerPrefs.GetInt(BestScoreKey, 0);

        if (UIManager.Instance != null)
            UIManager.Instance.ShowGameOver(score, bestScore);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayGameOverSound(isNewBest);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}