using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private Rigidbody rb;
    private BowController owner;
    private bool hasHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetOwner(BowController bow)
    {
        owner = bow;
    }

    void Update()
    {
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Fruit"))
        {
            hasHit = true;
            GameManager.Instance.AddScore(10);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bomb"))
        {
            hasHit = true;
            Destroy(other.gameObject);
            Destroy(gameObject);
            GameManager.Instance.TriggerGameOver();
        }
        else if (other.CompareTag("Ground"))
        {
            hasHit = true;
            Destroy(gameObject, 0.5f);  // brief delay so you see it stick
        }
    }
}