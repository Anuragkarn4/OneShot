using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBombSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject applePrefab;
    public GameObject bombPrefab;

    [Header("Spawn Area")]
    public float topY = 6f;         // height above ground to spawn
    public float minX = -8f;        // left boundary
    public float maxX = 8f;         // right boundary

    [Header("Timing")]
    public float minInterval = 0.5f;
    public float maxInterval = 1.5f;

    [Header("Ratios")]
    [Range(0f, 1f)]
    public float bombChance = 0.2f; // 20% bombs, 80% apples

    [Header("Fall Speed")]
    public float minFallForce = 5f;
    public float maxFallForce = 8f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    System.Collections.IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (!GameManager.Instance.isGameOver)
            {
                SpawnOne();
            }

            float wait = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnOne()
    {
        if (applePrefab == null || bombPrefab == null) return;

        // Decide whether this spawn is apple or bomb
        bool spawnBomb = (Random.value < bombChance);

        GameObject prefab = spawnBomb ? bombPrefab : applePrefab;

        // Random X along the top
        float x = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(x, topY, 0f);

        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Give it a downward impulse so it starts falling
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.down * Random.Range(minFallForce, maxFallForce);
        }
    }
}
