using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;
    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;
        score += points;
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateScore(score);
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;
        if (UIManager.Instance != null)
            UIManager.Instance.ShowGameOver(score);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}