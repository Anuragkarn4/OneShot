using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    [Header("Settings")]
    public string gameSceneName = "GameScene"; // must match your scene name exactly
    public TextMeshProUGUI titleText;
    public float titleBobSpeed = 1.5f;
    public float titleBobAmount = 10f;

    private Vector3 titleStartPos;

    void Start()
    {
        if (titleText != null)
            titleStartPos = titleText.transform.position;
    }

    void Update()
    {
        // Gently bob the title up and down
        if (titleText != null)
        {
            float newY = titleStartPos.y +
                         Mathf.Sin(Time.time * titleBobSpeed) * titleBobAmount;
            titleText.transform.position = new Vector3(
                titleStartPos.x, newY, titleStartPos.z);
        }
    }

    public void OnPlayButtonClicked()
    {
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(gameSceneName);
    }
}
