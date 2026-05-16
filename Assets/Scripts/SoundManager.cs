using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sound Clips")]
    public AudioClip arrowShootSound;
    public AudioClip fruitHitSound;
    public AudioClip bombExplosionSound;
    public AudioClip sadGameOverSound;
    public AudioClip happyNewBestSound;

    private AudioSource audioSource;
    private AudioSource gameOverSource; 

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        gameOverSource = gameObject.AddComponent<AudioSource>();
        gameOverSource.playOnAwake = false;
    }

    public void PlayArrowShoot()
    {
        if (arrowShootSound != null)
            audioSource.PlayOneShot(arrowShootSound);
    }

    public void PlayFruitHit()
    {
        if (fruitHitSound != null)
            audioSource.PlayOneShot(fruitHitSound);
    }

    public void PlayBombExplosion()
    {
        if (bombExplosionSound != null)
            audioSource.PlayOneShot(bombExplosionSound);
    }

    public void PlayGameOverSound(bool isNewBestScore)
    {
        if (isNewBestScore)
        {
            if (happyNewBestSound != null)
            {
                gameOverSource.clip = happyNewBestSound;
                gameOverSource.Play();
            }
        }
        else
        {
            if (sadGameOverSound != null)
            {
                gameOverSource.clip = sadGameOverSound;
                gameOverSource.Play();
            }
        }
    }
}